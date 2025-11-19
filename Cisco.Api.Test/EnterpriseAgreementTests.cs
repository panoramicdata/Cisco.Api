using Cisco.Api.Data.EnterpriseAgreement.Responses;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class EnterpriseAgreementTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async void GetConsumptionReport_Succeeds()
	{
		try
		{
			var domain = Config.SmartAccountDomainReal;

			var response = await CiscoClient
				.EnterpriseAgreement
				.GetConsumptionReportForAllSubscriptionsAssociatedWithSmartAccountDomainAsync(domain)
				.ConfigureAwait(true);

			response.Subscriptions.Should().BeOfType<List<Subscription>>();
			response.Subscriptions.Should().NotBeEmpty();


		}
		catch (Exception ex)
		{
			iTestOutputHelper.WriteLine($"Exception: {ex.Message}");
			throw;
		}
	}
}
