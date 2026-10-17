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
	{
		var device = new DeviceDetail();
		device.DeviceId = "d1";
		device.HostName = "host";
		device.IpAddress = "1.2.3.4";
		device.OriginalSerialNumber = "sn1";
		device.ValidatedSerialNumber = "sn2";
		device.ProductId = "pid";
		device.ProductDescription = "desc";
		device.ProductFamily = "fam";
		device.ProductName = "name";
		device.ProductModel = "model";
		device.ElementType = "el";
		device.OriginalInventoryDate = new DateTime(2024, 1, 2);
		device.LastInventoryDate = new DateTime(2024, 3, 4);
		device.BaseProductId = "bpid";
		device.InstalledMemory = "8GB";
		device.HardWareVersion = "hw1";
		device.SoftwareVersion = "sw1";
		device.SoftwareType = "ios";
		device.FlashMemory = "4GB";
		device.Manufacturer = "cisco";
		device.C3InstanceId = "c3";
		device.ShipDate = "2023-01-01";
		device.OsVersion = "os1";
		device.SelectionType = "sel";
		device.EquipmentType = "eq";
		device.CardDetails = [new CardDetail { CardId = "c1" }];

		device.DeviceId.Should().Be("d1");
		device.HostName.Should().Be("host");
		device.IpAddress.Should().Be("1.2.3.4");
		device.OriginalSerialNumber.Should().Be("sn1");
		device.ValidatedSerialNumber.Should().Be("sn2");
		device.ProductId.Should().Be("pid");
		device.ProductDescription.Should().Be("desc");
		device.ProductFamily.Should().Be("fam");
		device.ProductName.Should().Be("name");
		device.ProductModel.Should().Be("model");
		device.ElementType.Should().Be("el");
		device.OriginalInventoryDate.Should().Be(new DateTime(2024, 1, 2));
		device.LastInventoryDate.Should().Be(new DateTime(2024, 3, 4));
		device.BaseProductId.Should().Be("bpid");
		device.InstalledMemory.Should().Be("8GB");
		device.HardWareVersion.Should().Be("hw1");
		device.SoftwareVersion.Should().Be("sw1");
		device.SoftwareType.Should().Be("ios");
		device.FlashMemory.Should().Be("4GB");
		device.Manufacturer.Should().Be("cisco");
		device.C3InstanceId.Should().Be("c3");
		device.ShipDate.Should().Be("2023-01-01");
		device.OsVersion.Should().Be("os1");
		device.SelectionType.Should().Be("sel");
		device.EquipmentType.Should().Be("eq");
		device.CardDetails.Should().HaveCount(1);
	}

	/// <summary>Every CardDetail property is set and read back.</summary>
	[Fact]
	public void CardDetail_EveryPropertyRoundTrips()
	{
		var card = new CardDetail();
		card.CardId = "c1";
		card.DeviceId = "d1";
		card.CardDescription = "desc";
		card.CardFamily = "fam";
		card.ProductId = "pid";
		card.ProductDescription = "pdesc";
		card.ProductFamily = "pfam";
		card.FruFlag = "1";
		card.OriginalSerialNumber = "sn1";
		card.ValidatedSerialNumber = "sn2";
		card.SlotNumber = "1";
		card.FirmwareVersionNumber = "fw";
		card.HardwareVersionNumber = "hw";
		card.SoftwareVersion = "sw";
		card.Manufacturer = "cisco";
		card.C3InstanceId = "c3";
		card.ShipDate = "2023-01-01";

		card.CardId.Should().Be("c1");
		card.DeviceId.Should().Be("d1");
		card.CardDescription.Should().Be("desc");
		card.CardFamily.Should().Be("fam");
		card.ProductId.Should().Be("pid");
		card.ProductDescription.Should().Be("pdesc");
		card.ProductFamily.Should().Be("pfam");
		card.FruFlag.Should().Be("1");
		card.OriginalSerialNumber.Should().Be("sn1");
		card.ValidatedSerialNumber.Should().Be("sn2");
		card.SlotNumber.Should().Be("1");
		card.FirmwareVersionNumber.Should().Be("fw");
		card.HardwareVersionNumber.Should().Be("hw");
		card.SoftwareVersion.Should().Be("sw");
		card.Manufacturer.Should().Be("cisco");
		card.C3InstanceId.Should().Be("c3");
		card.ShipDate.Should().Be("2023-01-01");
	}
}
