using Cisco.Api.Interfaces;
using Refit;
using System.Net;
using System.Reflection;
using System.Text;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Builds an HTTP request for every Refit method in the library, offline.
/// </summary>
/// <remarks>
/// <para>
/// MS-26455: <see cref="IEox.GetBySerialNumberAsync(string, int, CancellationToken)"/> carried
/// <c>[Body]</c> on the one parameter its URL template also needed. Refit 13 let a single parameter
/// do both jobs, so the request was correct and the mismatch stayed invisible. Refit 15 does not,
/// and every Cisco EoX lookup by serial number began failing with "URL ... has parameter
/// {serialNumber}, but no method parameter matches" before the call left the process.
/// </para>
/// <para>
/// Constructing a client does not catch that, because Refit validates a URL template only when a
/// request is built. These tests therefore invoke every method against a handler that captures the
/// request instead of sending it, so they need no Cisco credentials and no network.
/// </para>
/// </remarks>
public class RefitRequestBuildingTests
{
	private const string SampleSerialNumber = "FOC2420R0PU";

	private const int SamplePageIndex = 1;

	private static readonly DateTime SampleDate = new(2026, 1, 1);

	private static readonly Uri BaseAddress = new("https://api.cisco.com");

	/// <summary>
	/// The settings <see cref="CiscoClient"/> itself uses, so these tests exercise the real
	/// configuration rather than a copy of it that can drift.
	/// </summary>
	private static readonly RefitSettings CiscoRefitSettings = CiscoClient.CreateRefitSettings();

	/// <summary>
	/// Verifies that every Refit method in the library can build a request.
	/// </summary>
	[Fact]
	public void EveryRefitMethod_BuildsARequest()
	{
		var refitMethods = GetRefitInterfaces()
			.SelectMany(refitInterface => GetRefitMethods(refitInterface)
				.Select(method => (RefitInterface: refitInterface, Method: method)))
			.ToList();

		// A sweep that found nothing would pass while proving nothing.
		refitMethods.Should().NotBeEmpty("the library exposes several Refit interfaces");

		var failures = new List<string>();

		foreach (var (refitInterface, method) in refitMethods)
		{
			try
			{
				BuildRequest(refitInterface, method);
			}
			catch (Exception exception)
			{
				failures.Add($"{refitInterface.Name}.{method.Name}: {Unwrap(exception).Message}");
			}
		}

		failures
			.Should()
			.BeEmpty("a method that cannot build a request throws the moment a caller uses it, and nothing in the build says so");
	}

	/// <summary>
	/// Verifies that the EoX serial number lookup puts the serial number in the path.
	/// </summary>
	[Fact]
	public void Eox_GetBySerialNumberAsync_PutsTheSerialNumberInThePathAndSendsNoBody()
	{
		// MS-26455: this is the URL that Magic Suite 4.5 sends in production today.
		var method = typeof(IEox).GetMethod(
			nameof(IEox.GetBySerialNumberAsync),
			[typeof(string), typeof(int), typeof(CancellationToken)]);

		method.Should().NotBeNull();

		var request = BuildRequest(typeof(IEox), method);

		request.Method.Should().Be(HttpMethod.Get);
		request.RequestUri.Should().NotBeNull();
		request.RequestUri!.AbsolutePath
			.Should()
			.Be($"/supporttools/eox/rest/5/EOXBySerialNumber/{SamplePageIndex}/{SampleSerialNumber}");
		request.Content.Should().BeNull("a GET carries no body, and [Body] here is what left the path unsatisfiable");
	}

	/// <summary>
	/// Invokes a Refit method against a handler that captures the request instead of sending it.
	/// </summary>
	private static HttpRequestMessage BuildRequest(Type refitInterface, MethodInfo method)
	{
		var handler = new RequestCapturingHandler();
		using var httpClient = new HttpClient(handler) { BaseAddress = BaseAddress };
		var client = RestService.For(refitInterface, httpClient, CiscoRefitSettings);
		var arguments = method
			.GetParameters()
			.Select(parameter => SampleValueFor(parameter.ParameterType))
			.ToArray();

		try
		{
			((Task)method.Invoke(client, arguments)!).GetAwaiter().GetResult();
		}
		catch (Exception) when (handler.Request is not null)
		{
			// The request reached the transport, which is all these tests prove. Whether the stub
			// response deserialises into the method's return type is not their business.
		}

		return handler.Request
			?? throw new InvalidOperationException($"{refitInterface.Name}.{method.Name} sent no request.");
	}

	/// <summary>
	/// A representative value for a parameter, good enough to build a request with.
	/// </summary>
	private static object? SampleValueFor(Type type)
	{
		if (type == typeof(CancellationToken))
		{
			return default(CancellationToken);
		}

		if (type == typeof(string))
		{
			return SampleSerialNumber;
		}

		if (typeof(IEnumerable<string>).IsAssignableFrom(type))
		{
			return new List<string> { SampleSerialNumber };
		}

		var valueType = Nullable.GetUnderlyingType(type) ?? type;

		if (TrySampleScalar(valueType, out var scalar))
		{
			return scalar;
		}

		return type.IsValueType || type.GetConstructor(Type.EmptyTypes) is not null
			? Activator.CreateInstance(type)
			: null;
	}

	/// <summary>
	/// A representative value for a numeric, date or enum type.
	/// </summary>
	private static bool TrySampleScalar(Type valueType, out object? value)
	{
		value = null;

		if (valueType == typeof(int))
		{
			value = SamplePageIndex;
		}
		else if (valueType == typeof(long))
		{
			value = (long)SamplePageIndex;
		}
		else if (valueType == typeof(DateTime))
		{
			value = SampleDate;
		}
		else if (valueType == typeof(DateTimeOffset))
		{
			value = new DateTimeOffset(SampleDate, TimeSpan.Zero);
		}
		else if (valueType.IsEnum)
		{
			value = Enum.GetValues(valueType).GetValue(0);
		}
		else
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Every interface in the library that Refit implements, internal ones included.
	/// </summary>
	private static IEnumerable<Type> GetRefitInterfaces()
		=> typeof(CiscoClient)
			.Assembly
			.GetTypes()
			.Where(type => type.IsInterface && GetRefitMethods(type).Any())
			.OrderBy(type => type.Name, StringComparer.Ordinal);

	/// <summary>
	/// The methods on an interface that carry a Refit HTTP method attribute.
	/// </summary>
	private static IEnumerable<MethodInfo> GetRefitMethods(Type refitInterface)
		=> refitInterface
			.GetMethods()
			.Where(method => method.GetCustomAttributes<HttpMethodAttribute>(true).Any());

	/// <summary>
	/// Strips the reflection and task wrappers from an exception so the message is the real one.
	/// </summary>
	private static Exception Unwrap(Exception exception)
	{
		while (exception is TargetInvocationException or AggregateException
			&& exception.InnerException is not null)
		{
			exception = exception.InnerException;
		}

		return exception;
	}

	/// <summary>
	/// Captures the request instead of sending it, and answers with an empty JSON document.
	/// </summary>
	private sealed class RequestCapturingHandler : HttpMessageHandler
	{
		public HttpRequestMessage? Request { get; private set; }

		protected override Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			Request = request;

			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				RequestMessage = request,
				Content = new StringContent("{}", Encoding.UTF8, "application/json")
			});
		}
	}
}
