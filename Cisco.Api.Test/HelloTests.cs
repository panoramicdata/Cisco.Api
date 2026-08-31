using Cisco.Api.Data.Hello;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Contains tests for hello operations.
/// </summary>
/// <param name="iTestOutputHelper">The test output helper.</param>
public class HelloTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	/// <summary>
	/// Verifies the hello succeeds scenario.
	/// </summary>
	[Fact]
	public async Task Hello_Succeeds()
	{
		var response = await CiscoClient
			.Hello
			.HelloAsync(default)
			.ConfigureAwait(true);
		response.Should().BeOfType<Response>();
		response.HelloResponse.Should().NotBeNull();
		response.HelloResponse.Response.Should().Be("Hello World!");
	}
}
