using Cisco.Api.Data.EnterpriseAgreement.Responses;
using System;
using System.Collections.Generic;
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
	public async Task GetConsumptionReport_Succeeds()
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
			_iTestOutputHelper.WriteLine($"Exception: {ex.Message}");
			throw;
		}
	}
}
