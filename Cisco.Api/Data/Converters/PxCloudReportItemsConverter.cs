namespace Cisco.Api.Data.Converters;
//internal class PxCloudReportItemsConverter : JsonCreationConverter<ReportPayloadItem>
//{
//	protected override ReportPayloadItem Create(Type objectType, JObject jObject)
//	{
//		var reportName = jObject["reportName"]?.Value<string>()?.ToLowerInvariant();
//		return reportName switch
//		{
//			"Assets" => new ReportPayloadItemsAssets(),
//			"Software" => new ReportPayloadItemsSoftware(),
//			"Hardware" => new ReportPayloadItemsHardware(),
//			"FieldNotices" => new ReportPayloadItemsFieldNotices(),
//			"ProrityBugs" => new ReportPayloadItemsPriorityBugs(),
//			"SecurityAdvisories" => new ReportPayloadItemsSecurityAdvisories(),
//			"PurchasedLicenses" => new ReportPayloadItemsPurchasedLicenses(),
//			"Licenses" => new ReportPayloadItemsLicensesWithAssets(),
//			_ => throw new NotSupportedException($"Unsupported report type: '{reportName}'."),
//		};
//	}

//	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
//		=> throw new NotSupportedException();
//}