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

		var eoxTask = Eox.GetByDatesAsync(yesterdayMidnight, todayMidnight, 1, cancellationToken);
		var helloTask = Hello.HelloAsync(cancellationToken);
		var psirtTask = Psirt.GetLatestAsync(1, cancellationToken);
		var productInfoTask = ProductInfo.GetBySerialNumbersAsync([], cancellationToken);
		var pssTask = Pss.GetFieldNoticesAsync(new FieldNoticesRequest(), cancellationToken);
		var serialNumberToInfoTask = SerialNumberToInfo.GetCoverageStatusBySerialNumbersAsync([], cancellationToken);
		var softwareSuggestionTask = SoftwareSuggestion.GetByProductIdsAsync(["C9200"], 1, cancellationToken);
		var umbrellaTask = Umbrella.ListSitesAsync(cancellationToken: cancellationToken);

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
