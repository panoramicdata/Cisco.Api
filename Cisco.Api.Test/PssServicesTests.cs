using Cisco.Api.Data.Pss;
using Cisco.Api.Implementations;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Exercises every <see cref="PssServices"/> method through a stub HTTP handler, so each
/// public method's body runs without any network or credentials.
/// </summary>
public class PssServicesTests
{
	private readonly PssServices _sut = new(new System.Net.Http.HttpClient(
		new CapturingHandler())
	{
		BaseAddress = new System.Uri("https://api.cisco.com/pss/v1.0/")
	});

	/// <summary>Each public method runs without throwing against the stub transport.</summary>
	[Fact]
	public async Task EveryPublicMethod_RunsAgainstTheStub()
	{
		var methods = new Func<Task>[]
		{
			async () => await _sut.GetCustomersInventoryIdsAsync(new CustomersInventoryRequest(), CancellationToken.None),
			async () => await _sut.GetCustomerInventoryDetailsAsync(new CustomerInventoryDetailsRequest(), CancellationToken.None),
			async () => await _sut.GetCustomerExtendedInventoryDetailsAsync(new CustomerExtendedInventoryDetailsRequest(), CancellationToken.None),
			async () => await _sut.GetCustomerInventoryPaginatedDetailsAsync(new CustomerInventoryDetailPaginatedRequest(), CancellationToken.None),
			async () => await _sut.GetContractCoverageAsync(new ContractCoverageRequest(), CancellationToken.None),
			async () => await _sut.GetSoftwareEoxAsync(new SoftwareEoxRequest(), CancellationToken.None),
			async () => await _sut.GetSoftwareEoxBulletinAsync(new SoftwareEoxBulletinRequest(), CancellationToken.None),
			async () => await _sut.GetHardwareEoxAsync(new HardwareEoxRequest(), CancellationToken.None),
			async () => await _sut.GetHardwareEoxBulletinAsync(new HardwareEoxBulletinRequest(), CancellationToken.None),
			async () => await _sut.GetEoswmLifecycleAsync(new EoswmLifecycleRequest(), CancellationToken.None),
			async () => await _sut.GetPsirtAsync(new PsirtRequest(), CancellationToken.None),
			async () => await _sut.GetPsirtDetailsAsync(new PsirtDetailsRequest(), CancellationToken.None),
			async () => await _sut.GetFieldNoticesAsync(new FieldNoticesRequest(), CancellationToken.None),
			async () => await _sut.GetFieldNoticesDetailsAsync(new FieldNoticesDetailsRequest(), CancellationToken.None)
		};

		for (var i = 0; i < methods.Length; i++)
		{
			await methods[i]();
			CapturingHandler.RequestCount.Should().Be(i + 1, $"method {i} should have sent exactly one request");
		}
	}

	private sealed class CapturingHandler : System.Net.Http.HttpMessageHandler
	{
		public static int RequestCount { get; private set; }

		protected override Task<System.Net.Http.HttpResponseMessage> SendAsync(
			System.Net.Http.HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			RequestCount++;
			return Task.FromResult(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK)
			{
				Content = new System.Net.Http.StringContent("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"/>")
			});
		}
	}
}
