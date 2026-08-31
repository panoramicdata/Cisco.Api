using Cisco.Api.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Contains tests for enterprise agreement operations.
/// </summary>
public class EnterpriseAgreementTests : Test
{
	private readonly ITestOutputHelper _iTestOutputHelper;

	/// <summary>
	/// Initializes a new instance of the <see cref="EnterpriseAgreementTests"/> class.
	/// </summary>
	public EnterpriseAgreementTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
		_iTestOutputHelper = iTestOutputHelper;
	}

	/// <summary>
	/// Verifies the get consumption report returns200 but with an error due to perms succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetConsumptionReport_Returns200ButWithAnErrorDueToPerms_Succeeds()
	{
		try
		{
			var domain = Config.SmartAccountDomainReal;

			var exception = await Assert.ThrowsAsync<CiscoApiException>(async () =>
			{
				await CiscoClient
					.EnterpriseAgreement
					.GetConsumptionReportForAllSubscriptionsAssociatedWithSmartAccountDomainAsync(domain, default)
					.ConfigureAwait(true);
			});

			_iTestOutputHelper.WriteLine($"Caught expected exception: {exception.Message}");
			exception.Message.Should().Contain("No Valid Subscriptions found");
		}
		catch (Exception ex)
		{
			_iTestOutputHelper.WriteLine($"Unexpected exception: {ex.Message}");
			throw;
		}
	}
}
