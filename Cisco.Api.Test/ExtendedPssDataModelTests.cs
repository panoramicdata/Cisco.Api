using Cisco.Api.Data.Pss;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip tests for the Extended device/card detail models.
/// </summary>
public class ExtendedPssDataModelTests
{
	/// <summary>Every ExtendedDeviceDetail property is set and read back.</summary>
	[Fact]
	public void ExtendedDeviceDetail_EveryPropertyRoundTrips()
	{
		var device = new ExtendedDeviceDetail
		{
			BootVersion = "b",
			FeatureSet = "fs",
			CategoryName = "cn",
			CategoryShortName = "csn",
			ItemType = "it",
			SnmpLocation = "snmp",
			ShipToAddress = "sta",
			InstallAtAddress = "iaa",
			BillToName = "b",
			SystemContact = "sc"
		};
		device.BootVersion.Should().Be("b");
		device.FeatureSet.Should().Be("fs");
		device.CategoryName.Should().Be("cn");
		device.CategoryShortName.Should().Be("csn");
		device.ItemType.Should().Be("it");
		device.SnmpLocation.Should().Be("snmp");
		device.ShipToAddress.Should().Be("sta");
		device.InstallAtAddress.Should().Be("iaa");
		device.BillToName.Should().Be("b");
		device.SystemContact.Should().Be("sc");
	}

	/// <summary>Every ExtendedCardDetail property is set and read back.</summary>
	[Fact]
	public void ExtendedCardDetail_EveryPropertyRoundTrips()
	{
		var card = new ExtendedCardDetail
		{
			CardType = "ct",
			BaseProductId = "bp",
			SelectionType = "st",
			CategoryName = "cn",
			CategoryShortName = "csn",
			ItemType = "it"
		};
		card.CardType.Should().Be("ct");
		card.BaseProductId.Should().Be("bp");
		card.SelectionType.Should().Be("st");
		card.CategoryName.Should().Be("cn");
		card.CategoryShortName.Should().Be("csn");
		card.ItemType.Should().Be("it");
	}
}
