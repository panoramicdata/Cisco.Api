using System;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public class EoxTests : Test
	{
		public EoxTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetEoxInfoByDatesAsync()
		{
			var eoxInfoPage = await CiscoClient.GetEoxInfoByDatesAsync(DateTime.Parse("2017-01-01"), DateTime.Parse("2018-01-01"));
			CheckEoxInfoPage(eoxInfoPage);
		}

		[Fact]
		public async void GetEoxInfoByProductIdAsync()
		{
			var eoxInfoPage = await CiscoClient.GetEoxInfoByProductIdAsync("WIC-1T=");
			CheckEoxInfoPage(eoxInfoPage);
		}

		[Fact]
		public async void GetEoxInfoBySerialNumberAsync()
		{
			var eoxRecord = await CiscoClient.GetEoxInfoBySerialNumberAsync("FTX1910100B");
			CheckEoxRecord(eoxRecord);
		}

		private static void CheckEoxInfoPage(EoxInfoPage eoxInfoPage)
		{
			Assert.NotNull(eoxInfoPage);
			Assert.NotNull(eoxInfoPage.EoxRecords);
			Assert.NotEmpty(eoxInfoPage.EoxRecords);
			Assert.All(eoxInfoPage.EoxRecords, CheckEoxRecord);
		}

		private static void CheckEoxRecord(EoxRecord eoxRecord)
		{
			Assert.NotNull(eoxRecord);
			Assert.NotNull(eoxRecord.EndOfRoutineFailureAnalysisDate);
			Assert.NotNull(eoxRecord.EndOfSaleDate);
			Assert.NotNull(eoxRecord.EndOfSecurityVulnerabilitySupportDate);
			Assert.NotNull(eoxRecord.EndOfServiceContractRenewalDate);
			Assert.NotNull(eoxRecord.EndOfSoftwareMaintenanceReleases);
			Assert.NotNull(eoxRecord.EolProductId);
			Assert.NotNull(eoxRecord.EndOfSvcAttachDate);
			Assert.NotNull(eoxRecord.ExternalAnnouncementDate);
			Assert.NotNull(eoxRecord.InputType);
			Assert.NotNull(eoxRecord.InputValue);
			Assert.NotNull(eoxRecord.LastSupportDate);
			Assert.NotNull(eoxRecord.LinkToProductBulletinUrl);
			Assert.NotNull(eoxRecord.ProductIdDescription);
			Assert.NotNull(eoxRecord.ProductBulletinNumber);
			Assert.NotNull(eoxRecord.UpdatedDate);

			Assert.NotNull(eoxRecord.EoxMigrationDetails);
			Assert.NotNull(eoxRecord.EoxMigrationDetails.Information);
			Assert.NotNull(eoxRecord.EoxMigrationDetails.Option);
			Assert.NotNull(eoxRecord.EoxMigrationDetails.ProductId);
			Assert.NotNull(eoxRecord.EoxMigrationDetails.ProductInfoUrl);
			Assert.NotNull(eoxRecord.EoxMigrationDetails.ProductName);
			Assert.NotNull(eoxRecord.EoxMigrationDetails.Strategy);
			Assert.NotNull(eoxRecord.EoxMigrationDetails.PidActiveFlag);
		}

		[Fact]
		public async void GetEoxInfoBySoftwareReleaseStringAsync()
		{
			var eoxInfoPage = await CiscoClient.GetEoxInfoBySoftwareReleaseStringAsync("12.2,IOS");
			Assert.NotNull(eoxInfoPage);
		}
	}
}
