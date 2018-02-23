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
			Assert.NotNull(eoxInfoPage);
		}

		[Fact]
		public async void GetEoxInfoByProductIdAsync()
		{
			var eoxInfoPage = await CiscoClient.GetEoxInfoByProductIdAsync("WIC-1T=");
			Assert.NotNull(eoxInfoPage);
		}

		[Fact]
		public async void GetEoxInfoBySerialNumberAsync()
		{
			var eoxRecord = await CiscoClient.GetEoxInfoBySerialNumberAsync("FTX1910100B");
			Assert.NotNull(eoxRecord);
		}

		[Fact]
		public async void GetEoxInfoBySoftwareReleaseStringAsync()
		{
			var eoxInfoPage = await CiscoClient.GetEoxInfoBySoftwareReleaseStringAsync("12.2,IOS");
			Assert.NotNull(eoxInfoPage);
		}
	}
}
