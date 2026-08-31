using Cisco.Api.Data.Eox;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Contains tests for EoX operations.
/// </summary>
/// <param name="iTestOutputHelper">The test output helper.</param>
public class EoxTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	/// <summary>
	/// Verifies the get by dates async succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetByDatesAsync_Succeeds()
	{
		var eoxInfoPage = await CiscoClient
			.Eox
			.GetByDatesAsync(
				DateTime.Parse("2017-01-01", CultureInfo.InvariantCulture),
				DateTime.Parse("2018-01-01", CultureInfo.InvariantCulture),
				1,
				default)
			.ConfigureAwait(true);
		CheckEoxInfoPage(eoxInfoPage);
	}

	/// <summary>
	/// Verifies the get by product ID async succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetByProductIdAsync_Succeeds()
	{
		var eoxInfoPage = await CiscoClient
			.Eox
			.GetByProductIdAsync("WIC-1T=", 1, default)
			.ConfigureAwait(true);
		CheckEoxInfoPage(eoxInfoPage);
	}

	/// <summary>
	/// Verifies the get by serial number async succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetBySerialNumberAsync_Succeeds()
	{
		var eoxInfoPage = await CiscoClient
			.Eox
			.GetBySerialNumberAsync("FTX1910100B", 1, default)
			.ConfigureAwait(true);
		CheckEoxInfoPage(eoxInfoPage);
	}

	/// <summary>
	/// Verifies the get by software release string async succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetBySoftwareReleaseStringAsync_Succeeds()
	{
		var eoxInfoPage = await CiscoClient
			.Eox
			.GetBySoftwareReleaseStringAsync(["12.2,IOS"], 1, default)
			.ConfigureAwait(true);
		CheckEoxInfoPage(eoxInfoPage);
	}

	private static void CheckEoxInfoPage(EoxInfoPage eoxInfoPage)
	{
		eoxInfoPage.Should().NotBeNull();
		eoxInfoPage.EoxRecords.Should().NotBeNullOrEmpty();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EndOfRoutineFailureAnalysisDate).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EndOfSaleDate).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EndOfSecurityVulnerabilitySupportDate).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EndOfServiceContractRenewalDate).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EndOfSoftwareMaintenanceReleases).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EolProductId).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EndOfServiceAttachDate).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.ExternalAnnouncementDate).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.InputType).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.InputValue).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.LastSupportDate).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.LinkToProductBulletinUrl).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.ProductDescription).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.ProductBulletinNumber).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.UpdatedDate).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EoxMigrationDetails).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EoxMigrationDetails.Information).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EoxMigrationDetails.Option).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EoxMigrationDetails.ProductId).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EoxMigrationDetails.ProductInfoUrl).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EoxMigrationDetails.ProductName).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EoxMigrationDetails.Strategy).Should().NotBeNull();
		eoxInfoPage.EoxRecords.Select(eoxRecord => eoxRecord.EoxMigrationDetails.PidActiveFlag).Should().NotBeNull();
	}
}
