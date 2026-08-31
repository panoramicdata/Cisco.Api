using System;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Contains tests for authentication operations.
/// </summary>
/// <param name="iTestOutputHelper">The test output helper.</param>
public class AuthenticationTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	/// <summary>
	/// Verifies the no client ID throws exception scenario.
	/// </summary>
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
			.HelloAsync(default)
			.ConfigureAwait(true);
		};

		act
			.Should()
			.ThrowAsync<ArgumentException>()
			.WithMessage("Options ClientId must be set (Parameter 'options')");
	}

	/// <summary>
	/// Verifies the no client secret throws exception scenario.
	/// </summary>
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
			.HelloAsync(default)
			.ConfigureAwait(true);
		};

		act
			.Should()
			.ThrowAsync<ArgumentException>()
			.WithMessage("Options ClientSecret must be set (Parameter 'options')");
	}

	/// <summary>
	/// Verifies the get API access async succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetApiAccessAsync_Succeeds()
	{
		var apiAccess = await CiscoClient
			.GetApiAccessAsync(default);

		apiAccess.Should().NotBeNull();
	}
}