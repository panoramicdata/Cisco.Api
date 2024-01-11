using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class AuthenticationTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
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
			.ThrowAsync<ArgumentException>()
			.WithMessage("Options ClientId must be set (Parameter 'options')");
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
			.ThrowAsync<ArgumentException>()
			.WithMessage("Options ClientSecret must be set (Parameter 'options')");
	}
}