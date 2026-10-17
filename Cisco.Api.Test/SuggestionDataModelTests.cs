using Cisco.Api.Data.SoftwareSuggestion;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip tests for the SoftwareSuggestion data model.
/// </summary>
public class SuggestionDataModelTests
{
	/// <summary>Every SoftwareSuggestion property is set and read back.</summary>
	[Fact]
	public void SoftwareSuggestion_EveryPropertyRoundTrips()
	{
		var suggestion = new SoftwareSuggestion
		{
			Id = 1,
			IsSuggested = "Y",
			ReleaseFormat1 = "r1",
			ReleaseFormat2 = "r2",
			ReleaseDate = "2024-01-01",
			MajorRelease = "mr",
			ReleaseTrain = "rt",
			ReleaseLifeCycle = "rlc",
			RelDispName = "r",
			TrainDispName = "t",
			ErrorDetails = new ErrorDetailsResponse
			{
				ErrorCode = "e",
				ErrorDescription = "d",
				SuggestedAction = "s",
				InputIdentifier = "i"
			}
		};
		suggestion.Id.Should().Be(1);
		suggestion.IsSuggested.Should().Be("Y");
		suggestion.ReleaseFormat1.Should().Be("r1");
		suggestion.ReleaseFormat2.Should().Be("r2");
		suggestion.ReleaseDate.Should().Be("2024-01-01");
		suggestion.MajorRelease.Should().Be("mr");
		suggestion.ReleaseTrain.Should().Be("rt");
		suggestion.ReleaseLifeCycle.Should().Be("rlc");
		suggestion.RelDispName.Should().Be("r");
		suggestion.TrainDispName.Should().Be("t");
		suggestion.ErrorDetails.Should().NotBeNull();
	}
}
