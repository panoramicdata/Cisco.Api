using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public class SoftwareSuggestionTests : Test
	{
		public SoftwareSuggestionTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async Task GetByProductNumbersAsync_Succeeds()
		{
			var softwareSuggestionPage =
				await CiscoClient
					.SoftwareSuggestion
					.GetByProductIdsAsync(new[] { "ASR-903", "N7K-C7018" })
				.ConfigureAwait(true);

			softwareSuggestionPage.Should().NotBeNull();

			softwareSuggestionPage.PaginationResponseRecord.Should().NotBeNull();
			softwareSuggestionPage.PaginationResponseRecord.LastIndex.Should().Be(1);
			softwareSuggestionPage.PaginationResponseRecord.PageIndex.Should().Be(1);
			softwareSuggestionPage.PaginationResponseRecord.PageRecords.Should().Be(4);
			softwareSuggestionPage.PaginationResponseRecord.TotalRecords.Should().Be(4);

			softwareSuggestionPage.Suggestions.Should().NotBeNull();
			softwareSuggestionPage.Suggestions.Select(suggestion => suggestion.Suggestions).Should().NotBeNull();
			softwareSuggestionPage.Suggestions.Select(suggestion => suggestion.Product).Should().NotBeNull();
			softwareSuggestionPage.Suggestions.Select(suggestion => suggestion.Id).Should().NotBeNullOrEmpty();
		}
	}
}