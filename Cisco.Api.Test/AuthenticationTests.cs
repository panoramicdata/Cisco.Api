using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public class AuthenticationTests : Test
	{
		public AuthenticationTests(ITestOutputHelper iTestOutputHelper)
			: base(iTestOutputHelper)
		{
		}

		[Fact]
		public void NoClientId_ThrowsException()
		{
			Func<Task> act = async () =>
			{
				await new CiscoClient(new CiscoClientOptions
				{
					ClientId = null,
					ClientSecret = "set"
				})
				.Hello
				.HelloAsync()
				.ConfigureAwait(false);
			};

			act
				.Should()
				.Throw<ArgumentException>()
				.WithMessage("Options ClientId must be set when no HttpClient is provided (Parameter 'options')");
		}
		[Fact]
		public void NoClientSecret_ThrowsException()
		{
			Func<Task> act = async () =>
			{
				await new CiscoClient(new CiscoClientOptions
				{
					ClientId = "set",
					ClientSecret = null
				})
				.Hello
				.HelloAsync()
				.ConfigureAwait(false);
			};

			act
				.Should()
				.Throw<ArgumentException>()
				.WithMessage("Options ClientSecret must be set when no HttpClient is provided (Parameter 'options')");
		}
	}
}