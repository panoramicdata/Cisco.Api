using Cisco.Api.Data.EnterpriseAgreement.Responses;
using Cisco.Api.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
	/*
	[Fact]
	public async Task GetConsumptionReport_InvalidDomain_ShowsStructuredError()
	{
		var invalidDomain = "invalid.smart.account.domain";
		var act = async () => await CiscoClient
			.EnterpriseAgreement
			.GetConsumptionReportForAllSubscriptionsAssociatedWithSmartAccountDomainAsync(invalidDomain)
			.ConfigureAwait(true);

		var ex = await FluentActions.Invoking(() => act())
			.Should()
			.ThrowAsync<CiscoApiException>();

		var ciscoEx = ex.Which;
		ciscoEx.Code.Should().Be(400001);
		ciscoEx.Severity.Should().Be("Error");
		ciscoEx.MoreInfo.Should().Contain("ela-it-support@cisco.com");
		ciscoEx.ApiMessage.Should().Contain(invalidDomain);
	}
	*/
}
