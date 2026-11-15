using Cisco.Api.Data.Pss;
using System.Xml;
using System.Xml.Serialization;
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

	/// <summary>Elements declared on the shared base class bind alongside the card's own elements.</summary>
	[Fact]
	public void CardDetail_XmlDeserialisation_BindsInheritedAndOwnElements()
	{
		const string xml = "<cardDetail><cardId>c1</cardId><productId>p1</productId><shipDate>2023-01-01</shipDate></cardDetail>";

		var card = Deserialize<CardDetail>(xml, "cardDetail");

		PropertyAssert.AssertValues(
			card!,
			(nameof(CardDetail.CardId), "c1"),
			(nameof(CardDetail.ProductId), "p1"),
			(nameof(CardDetail.ShipDate), "2023-01-01"));
	}

	/// <summary>Elements declared on the shared base class bind alongside the device's own elements.</summary>
	[Fact]
	public void DeviceDetail_XmlDeserialisation_BindsInheritedAndOwnElements()
	{
		const string xml = "<deviceDetail xmlns=\"http://www.cisco.com/InventoryService\"><deviceId>d1</deviceId><manufacturer>cisco</manufacturer><cardDetail><cardId>c1</cardId></cardDetail></deviceDetail>";

		var device = Deserialize<DeviceDetail>(xml, "deviceDetail", "http://www.cisco.com/InventoryService");

		PropertyAssert.AssertValues(
			device!,
			(nameof(DeviceDetail.DeviceId), "d1"),
			(nameof(DeviceDetail.Manufacturer), "cisco"));
		device!.CardDetails.Should().ContainSingle().Which.CardId.Should().Be("c1");
	}

	private static T? Deserialize<T>(string xml, string root, string xmlNamespace = "") where T : class
	{
		using var stringReader = new StringReader(xml);
		using var reader = XmlReader.Create(stringReader, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null });
		return new XmlSerializer(typeof(T), new XmlRootAttribute(root) { Namespace = xmlNamespace }).Deserialize(reader) as T;
	}
}
