using Cisco.Api.Data.Pss;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip tests for the PSS data models whose properties are otherwise only exercised by
/// integration tests that need live credentials.
/// </summary>
public class PssDataModelTests
{
	/// <summary>Every string, nullable-string and DateTime property is set and read back.</summary>
	[Fact]
	public void DeviceDetail_EveryPropertyRoundTrips()
		=> PropertyAssert.SetAndRead<DeviceDetail>(
			(nameof(DeviceDetail.DeviceId), "d1"),
			(nameof(DeviceDetail.HostName), "host"),
			(nameof(DeviceDetail.IpAddress), "ip-address-value"),
			(nameof(DeviceDetail.OriginalSerialNumber), "sn1"),
			(nameof(DeviceDetail.ValidatedSerialNumber), "sn2"),
			(nameof(DeviceDetail.ProductId), "pid"),
			(nameof(DeviceDetail.ProductDescription), "desc"),
			(nameof(DeviceDetail.ProductFamily), "fam"),
			(nameof(DeviceDetail.ProductName), "name"),
			(nameof(DeviceDetail.ProductModel), "model"),
			(nameof(DeviceDetail.ElementType), "el"),
			(nameof(DeviceDetail.OriginalInventoryDate), new DateTime(2024, 1, 2)),
			(nameof(DeviceDetail.LastInventoryDate), new DateTime(2024, 3, 4)),
			(nameof(DeviceDetail.BaseProductId), "bpid"),
			(nameof(DeviceDetail.InstalledMemory), "8GB"),
			(nameof(DeviceDetail.HardWareVersion), "hw1"),
			(nameof(DeviceDetail.SoftwareVersion), "sw1"),
			(nameof(DeviceDetail.SoftwareType), "ios"),
			(nameof(DeviceDetail.FlashMemory), "4GB"),
			(nameof(DeviceDetail.Manufacturer), "cisco"),
			(nameof(DeviceDetail.C3InstanceId), "c3"),
			(nameof(DeviceDetail.ShipDate), "2023-01-01"),
			(nameof(DeviceDetail.OsVersion), "os1"),
			(nameof(DeviceDetail.SelectionType), "sel"),
			(nameof(DeviceDetail.EquipmentType), "eq"),
			(nameof(DeviceDetail.CardDetails), new List<CardDetail> { new() { CardId = "c1" } }));

	/// <summary>Every CardDetail property is set and read back.</summary>
	[Fact]
	public void CardDetail_EveryPropertyRoundTrips()
		=> PropertyAssert.SetAndRead<CardDetail>(
			(nameof(CardDetail.CardId), "c1"),
			(nameof(CardDetail.DeviceId), "d1"),
			(nameof(CardDetail.CardDescription), "desc"),
			(nameof(CardDetail.CardFamily), "fam"),
			(nameof(CardDetail.ProductId), "pid"),
			(nameof(CardDetail.ProductDescription), "pdesc"),
			(nameof(CardDetail.ProductFamily), "pfam"),
			(nameof(CardDetail.FruFlag), "1"),
			(nameof(CardDetail.OriginalSerialNumber), "sn1"),
			(nameof(CardDetail.ValidatedSerialNumber), "sn2"),
			(nameof(CardDetail.SlotNumber), "1"),
			(nameof(CardDetail.FirmwareVersionNumber), "fw"),
			(nameof(CardDetail.HardwareVersionNumber), "hw"),
			(nameof(CardDetail.SoftwareVersion), "sw"),
			(nameof(CardDetail.Manufacturer), "cisco"),
			(nameof(CardDetail.C3InstanceId), "c3"),
			(nameof(CardDetail.ShipDate), "2023-01-01"));
}
