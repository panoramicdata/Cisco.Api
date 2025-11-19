using Cisco.Api.Data.Pss;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api;
public partial class CiscoClient
{
	/// <summary>
	/// This method tries each Interface to see if the provided credentials are valid
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<ApiAccess> GetApiAccessAsync(CancellationToken cancellationToken)
	{
		_logger.LogDebug("Checking API access...");
		var apiAccess = new ApiAccess();
		var todayMidnight = DateTime.Today;
		var yesterdayMidnight = todayMidnight.AddDays(-1);

		// MS-19906 You can search specifically for the demo domain and in that specific case you may get an error depending on your perms but it's a 200
		var enterpriseAgreementTask = EnterpriseAgreement.GetConsumptionReportForAllSubscriptionsAssociatedWithSmartAccountDomainAsync("demo.mule.cisco.com", cancellationToken);
		var eoxTask = Eox.GetByDatesAsync(yesterdayMidnight, todayMidnight, 1, cancellationToken);
		var helloTask = Hello.HelloAsync(cancellationToken);
		var psirtTask = Psirt.GetLatestAsync(1, cancellationToken);
		// MS-19906 You cannot use an empty list - you get an API error, but you CAN make one up and if this is supported we get a response
		var productInfoTask = ProductInfo.GetBySerialNumbersAsync(["123"], cancellationToken);
		var pssTask = Pss.GetFieldNoticesAsync(new FieldNoticesRequest(), cancellationToken);
		// MS-19906 You cannot use an empty list - you get an API error, but you CAN make one up and if this is supported we get a response
		var serialNumberToInfoTask = SerialNumberToInfo.GetCoverageStatusBySerialNumbersAsync(["123"], cancellationToken);
		var softwareSuggestionTask = SoftwareSuggestion.GetByProductIdsAsync(["C9200"], 1, cancellationToken);
		// MS-19906 You can search for anything
		var smartAccountsAndLicensingTask = SmartAccountsAndLicensing.SearchSmartAccountsAsync(name: "123", cancellationToken: cancellationToken);
		var umbrellaTask = Umbrella.ListSitesAsync(cancellationToken: cancellationToken);

		try
		{
			_logger.LogDebug("Checking Enterprise Agreement");
			await enterpriseAgreementTask
				.ConfigureAwait(false);
			_logger.LogDebug("Enterprise Agreement succeeded");
			apiAccess.EnterpriseAgreement = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "Enterprise Agreement failed");
		}

		try
		{
			_logger.LogDebug("Checking Eox");
			await eoxTask
				.ConfigureAwait(false);
			_logger.LogDebug("Eox succeeded");
			apiAccess.Eox = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "Eox failed");
		}

		try
		{
			_logger.LogDebug("Checking Hello");
			await helloTask.ConfigureAwait(false);
			_logger.LogDebug("Hello succeeded");
			apiAccess.Hello = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "Hello failed");
		}

		try
		{
			_logger.LogDebug("Checking Psirt");
			await psirtTask.ConfigureAwait(false);
			apiAccess.Psirt = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "Psirt failed");
		}

		try
		{
			_logger.LogDebug("Checking ProductInfo");
			await productInfoTask.ConfigureAwait(false);
			_logger.LogDebug("ProductInfo succeeded");
			apiAccess.ProductInfo = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "ProductInfo failed");
		}

		try
		{
			_logger.LogDebug("Checking Pss");
			await pssTask
				.ConfigureAwait(false);
			_logger.LogDebug("Pss succeeded");
			apiAccess.Pss = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "Pss failed");
		}

		try
		{
			_logger.LogDebug("Checking SerialNumberToInfo");
			await serialNumberToInfoTask.ConfigureAwait(false);
			_logger.LogDebug("SerialNumberToInfo succeeded");
			apiAccess.SerialNumberToInfo = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "SerialNumberToInfo failed");
		}

		try
		{
			_logger.LogDebug("Checking SoftwareSuggestion");
			await softwareSuggestionTask
				.ConfigureAwait(false);
			_logger.LogDebug("SoftwareSuggestion succeeded");
			apiAccess.SoftwareSuggestion = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "SoftwareSuggestion failed");
		}

		try
		{
			_logger.LogDebug("Checking SmartAccountsAndLicensing");
			await smartAccountsAndLicensingTask
				.ConfigureAwait(false);
			_logger.LogDebug("SmartAccountsAndLicensing succeeded");
			apiAccess.SmartAccountsAndLicensing = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "SmartAccountsAndLicensing failed");
		}

		try
		{
			_logger.LogDebug("Checking Umbrella");
			await umbrellaTask
				.ConfigureAwait(false);
			_logger.LogDebug("Umbrella succeeded");
			apiAccess.Umbrella = true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "Umbrella failed");
		}

		return apiAccess;
	}
}
