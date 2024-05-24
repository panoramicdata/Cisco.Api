using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ContractDetail
{
	/// <summary>
	/// Name of the end customer.
	/// </summary>
	[DataMember(Name = "customerName")]
	public required string CustomerName { get; set; }

	/// <summary>
	/// Unique identifier of the customer.
	/// </summary>
	[DataMember(Name = "customerId")]
	public required string CustomerId { get; set; }

	/// <summary>
	/// Customer global ultimate name.
	/// </summary>
	[DataMember(Name = "customerGUName")]
	public required string CustomerGUName { get; set; }

	/// <summary>
	/// Customer Head Quarter Name.
	/// </summary>
	[DataMember(Name = "customerHQName")]
	public required string CustomerHQName { get; set; }

	/// <summary>
	/// Contract ID of the service contract.
	/// </summary>
	[DataMember(Name = "contractNumber")]
	public required string ContractNumber { get; set; }

	/// <summary>
	/// List of unique identifier for the Success Tracks.
	/// </summary>
	[DataMember(Name = "serviceTrackIds")]
	public List<int> ServiceTrackIds { get; set; } = [];

	/// <summary>
	/// Name of the Product.
	/// </summary>
	[DataMember(Name = "productId")]
	public required string ProductId { get; set; }

	/// <summary>
	/// Serial number of the contract.
	/// </summary>
	[DataMember(Name = "serialNumber")]
	public required string SerialNumber { get; set; }

	/// <summary>
	/// Contract Line Item type. Values will be PARENT,CHILD or STANDALONE.
	/// </summary>
	[DataMember(Name = "componentType")]
	public required string ComponentType { get; set; }

	/// <summary>
	/// Serial number of the contract line item.
	/// </summary>
	[DataMember(Name = "serviceLevel")]
	public required string ServiceLevel { get; set; }

	/// <summary>
	/// The date when the contract begins. UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "coverageStartDate")]
	public required String CoverageStartDate { get; set; }

	/// <summary>
	/// The date when the contract begins. UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "coverageEndDate")]
	public required String CoverageEndDate { get; set; }

	/// <summary>
	/// Installation Quantity.
	/// </summary>
	[DataMember(Name = "installationQuantity")]
	public int InstallationQuantity { get; set; }

	/// <summary>
	/// The Primary Key of Installed Product, provides a unique record per device installation.
	/// </summary>
	[DataMember(Name = "instanceNumber")]
	public int InstanceNumber { get; set; }
}
