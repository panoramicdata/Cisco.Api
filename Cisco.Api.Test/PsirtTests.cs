using Cisco.Api.Data.Psirt;
using FluentAssertions;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class PsirtTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async void GetPsirtByCveId()
	{
		var advisoryResponse = await CiscoClient
			.Psirt
			.GetByCveIdAsync("CVE-2020-3433", CancellationToken.None)
			.ConfigureAwait(true);

		advisoryResponse.Should().NotBeNull();
		advisoryResponse.Should().BeOfType<AdvisoriesResponse>();
		advisoryResponse.Advisories.Should().NotBeNull();
		advisoryResponse.Advisories.Should().NotBeEmpty();
	}

	[Fact]
	public async void GetAllPsirts()
	{
		var advisoryResponse = await CiscoClient
			.Psirt
			.GetAllAsync(CancellationToken.None)
			.ConfigureAwait(true);

		advisoryResponse.Should().NotBeNull();
		advisoryResponse.Should().BeOfType<AdvisoriesResponse>();
		advisoryResponse.Advisories.Should().NotBeNull();
		advisoryResponse.Advisories.Should().NotBeEmpty();
		advisoryResponse.Advisories.Count.Should().BeGreaterThan(100);
	}
}