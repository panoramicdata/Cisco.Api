using Cisco.Api.Data.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Exercises the JsonCreationConverter base class through a minimal concrete subclass, so the
/// abstract helper's ReadJson and CanConvert logic runs without a live service.
/// </summary>
public class JsonCreationConverterTests
{
	private sealed class DerivedConverter : JsonCreationConverter<Simple>
	{
		protected override Simple Create(Type objectType, JObject jObject)
			=> new Simple { CreatedBy = true };

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
			=> throw new NotSupportedException();
	}

	private sealed class Simple
	{
		public bool CreatedBy { get; set; }
		public string Value { get; set; } = string.Empty;
	}

	/// <summary>CanConvert reports true for assignable types and false otherwise.</summary>
	[Fact]
	public void CanConvert_MatchesOnAssignableTypes()
	{
		var converter = new DerivedConverter();
		converter.CanConvert(typeof(Simple)).Should().BeTrue();
		converter.CanConvert(typeof(string)).Should().BeFalse();
	}

	/// <summary>CanWrite is always false for this base converter.</summary>
	[Fact]
	public void CanWrite_IsFalse()
	{
		new DerivedConverter().CanWrite.Should().BeFalse();
	}

	/// <summary>ReadJson on a JSON null returns null. The reader must be advanced past the null
	/// token first, because the base implementation inspects the current token.</summary>
	[Fact]
	public void ReadJson_NullToken_ReturnsNull()
	{
		var converter = new DerivedConverter();
		using var reader = new JsonTextReader(new StringReader("null"))
		{
			DateParseHandling = DateParseHandling.None
		};
		reader.Read().Should().BeTrue();
		converter.ReadJson(reader, typeof(Simple), null, JsonSerializer.Create()).Should().BeNull();
	}

	/// <summary>ReadJson on a JSON object returns the target produced by Create.</summary>
	[Fact]
	public void ReadJson_Object_ReturnsCreatedTarget()
	{
		var converter = new DerivedConverter();
		using var reader = new JsonTextReader(new StringReader("""{"Value":"x"}"""));
		var result = (Simple?)converter.ReadJson(reader, typeof(Simple), null, JsonSerializer.Create());
		result.Should().NotBeNull();
		result!.CreatedBy.Should().BeTrue();
	}

	/// <summary>ReadJson on an object for which Create returns null yields null.</summary>
	[Fact]
	public void ReadJson_CreateReturnsNull_YieldsNull()
	{
		var converter = new NullReturningConverter();
		using var reader = new JsonTextReader(new StringReader("""{}"""));
		converter.ReadJson(reader, typeof(Simple), null, JsonSerializer.Create()).Should().BeNull();
	}

	private sealed class NullReturningConverter : JsonCreationConverter<Simple>
	{
		// The base class declares a non-nullable Create, but returning null is the supported way to
		// tell ReadJson that the object should not be materialised.
		#pragma warning disable CS8764
		protected override Simple? Create(Type objectType, JObject jObject) => null;
		#pragma warning restore CS8764

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
			=> throw new NotSupportedException();
	}
}
