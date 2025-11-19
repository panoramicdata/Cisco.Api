using Cisco.Api.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class EnterpriseAgreementTests : Test
{
	private readonly ITestOutputHelper _iTestOutputHelper;

	public EnterpriseAgreementTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
		_iTestOutputHelper = iTestOutputHelper;
	}

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
					.GetConsumptionReportForAllSubscriptionsAssociatedWithSmartAccountDomainAsync(domain)
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
