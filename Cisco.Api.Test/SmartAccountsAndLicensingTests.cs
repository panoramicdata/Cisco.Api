using Cisco.Api.Data.SmartAccountsAndLicensing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class SmartAccountsAndLicensingTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
    [Fact]
    public async void ListSmartAccounts_Succeeds()
    {
		var domain = Config.SmartAccountDomainReal;

		var response = await CiscoClient
            .SmartAccountsAndLicensing
            .ListSmartAccountsAsync(domain)
            .ConfigureAwait(true);

        response.Should().BeOfType<ListOfSmartAccountsResponse>();
        response.Accounts.Should().NotBeEmpty();
    }
}
