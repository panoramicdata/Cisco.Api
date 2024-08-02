using Cisco.Api.Data.Hello;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class HelloTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async Task Hello_Succeeds()
	{
		var response = await CiscoClient
			.Hello
			.HelloAsync()
			.ConfigureAwait(true);
		response.Should().BeOfType<Response>();
		response.HelloResponse.Should().NotBeNull();
		response.HelloResponse.Response.Should().Be("Hello World!");
	}
}
