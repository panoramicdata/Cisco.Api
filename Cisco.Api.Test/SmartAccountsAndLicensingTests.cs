using Cisco.Api.Data.SmartAccountsAndLicensing;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Contains tests for smart accounts and licensing operations.
/// </summary>
/// <param name="iTestOutputHelper">The test output helper.</param>
public class SmartAccountsAndLicensingTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
    /// <summary>
    /// Verifies the list smart accounts succeeds scenario.
    /// </summary>
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
