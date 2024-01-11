using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

public class ExtendedCardDetail : CardDetail
{
	/// <summary>
	/// The type of card associated with a device. e.g. Module
	/// </summary>
	[XmlElement("cardType")]
	public string CardType { get; set; } = null!;

	/// <summary>
	/// The type of card associated with a device. e.g. Module
	/// </summary>
	[XmlElement("baseProductId")]
	public string BaseProductId { get; set; } = null!;

	/// <summary>
	/// The type of card associated with a device. e.g. Module
	/// </summary>
	[XmlElement("selectionType")]
	public string SelectionType { get; set; } = null!;

	/// <summary>
	/// The type of card associated with a device. e.g. Module
	/// </summary>
	[XmlElement("categoryName")]
	public string CategoryName { get; set; } = null!;

	/// <summary>
	/// The type of card associated with a device. e.g. Module
	/// </summary>
	[XmlElement("categoryShortName")]
	public string CategoryShortName { get; set; } = null!;

	/// <summary>
	/// The type of card associated with a device. e.g. Module
	/// </summary>
	[XmlElement("itemType")]
	public string ItemType { get; set; } = null!;
}