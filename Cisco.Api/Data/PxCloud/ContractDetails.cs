﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ContractDetails
{
	/// <summary>
	/// The list of contract details.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ContractDetails> Items { get; set; } = null!;
}
