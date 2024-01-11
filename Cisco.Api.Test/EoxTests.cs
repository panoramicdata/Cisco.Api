using Cisco.Api.Data.Eox;
using FluentAssertions;
using System;
using System.Globalization;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class EoxTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async void GetByDatesAsync_Succeeds()
	{
		var eoxInfoPage = await CiscoClient
			.Eox
			.GetByDatesAsync(
				DateTime.Parse("2017-01-01", CultureInfo.InvariantCulture),
				DateTime.Parse("2018-01-01", CultureInfo.InvariantCulture))
			.ConfigureAwait(true);
		CheckEoxInfoPage(eoxInfoPage);
	}

	[Fact]
	public async void GetByProductIdAsync_Succeeds()
	{
		var eoxInfoPage = await CiscoClient
			.Eox
			.GetByProductIdAsync("WIC-1T=")
			.ConfigureAwait(true);
		CheckEoxInfoPage(eoxInfoPage);
	}

	[Fact]
	public async void GetBySerialNumberAsync_Succeeds()
	{
		var eoxInfoPage = await CiscoClient
			.Eox
			.GetBySerialNumberAsync("FTX1910100B")
			.ConfigureAwait(true);
		CheckEoxInfoPage(eoxInfoPage);
	}

	[Fact]
	public async void GetBySoftwareReleaseStringAsync_Succeeds()
	{
		var eoxInfoPage = await CiscoClient
			.Eox
			.GetBySoftwareReleaseStringAsync(new[] { "12.2,IOS" })
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
