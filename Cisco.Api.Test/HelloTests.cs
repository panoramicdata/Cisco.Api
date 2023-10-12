using System.Threading.Tasks;
using Cisco.Api.Data.Hello;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public class HelloTests : Test
	{
		public HelloTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async Task Hello_Succeeds()
		{
			var response = await CiscoClient
				.Hello
				.HelloAsync()
				.ConfigureAwait(false);
			response.Should().BeOfType<Response>();
			response.HelloResponse.Should().NotBeNull();
			response.HelloResponse.Response.Should().Be("Hello World!");
		}
	}
}
