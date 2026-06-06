using Cisco.Api.Data.SmartAccountsAndLicensing;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

public class SmartAccountsAndLicensingTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
    [Fact]
    public async Task ListSmartAccounts_Succeeds()
    {
		var domain = Config.SmartAccountDomainReal;

		var response = await CiscoClient
			.SmartAccountsAndLicensing
			.ListSmartAccountsAsync(domain, default)
			.ConfigureAwait(true);

        response.Should().BeOfType<ListOfSmartAccountsResponse>();
        response.Accounts.Should().NotBeEmpty();
    }
}
