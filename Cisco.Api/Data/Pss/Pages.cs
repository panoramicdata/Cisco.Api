using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// Paging information
	/// </summary>
	[DebuggerDisplay("{PageCount}/{PageTotal}")]
	[XmlRoot("pages", Namespace = "http://www.cisco.com/InventoryService")]
	public class Pages
	{
		/// <summary>
		/// The chunk size for each response returned.
		/// </summary>
		[XmlElement("pages")]
		public int PageSize { get; set; }

		/// <summary>
		/// The current page being returned.
		/// </summary>
		[XmlElement("pageCurrent")]
		public int PageCurrent { get; set; }

		/// <summary>
		/// The total number of pages which can be returned.
		/// </summary>
		[XmlElement("pageTotal")]
		public int PageTotal { get; set; }

		/// <summary>
		/// The total number of PIDs returned by this request.
		/// </summary>
		[XmlElement("pidCount")]
		public int PidCount { get; set; }
	}
}