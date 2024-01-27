using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public class SecurityAdvisoryTests : Test
	{
		public SecurityAdvisoryTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async Task GetByTypeAndVersionAsync_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByOsTypeAndVersion("asa", "9.8.2")
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-asa-ssl-vpn-Y88QOm77");
		}

		[Fact]
		public async Task GetByTypeAndVersionWithAliasAsync_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByOsTypeAndVersion("asa", "9.8.2", "ASAV")
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-asa-ssl-vpn-Y88QOm77");
		}

		[Fact]
		public async Task GetByProductCisco_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByProduct("Cisco")
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-cucm-rce-bWNzQcUm");
		}


		[Fact]
		public async Task GetByCveNameAsync_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByCveName("CVE-2022-20623")
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-nxos-bfd-dos-wGQXrzxn");
		}

		[Fact]
		public async Task GetLatestByIdAsync_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetLatesAdvisoriesLimitedByCount(15)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-tms-portal-xss-AXNeVg3s");
		}

		[Fact]
		public async Task GetOsVersionDataByIosTypeAsync_Succeeds()
		{
			var versions =
				await CiscoClient
					.SecurityAdvisory
					.GetOsVersionDataByType("ios")
				.ConfigureAwait(true);

			versions.Should().NotBeNull();
			foreach (var version in versions)
			{
				version.NosType.Should().Be("IOS");
				version.NosVersion.Should().NotBeNullOrEmpty();
			}
		}

		[Fact]
		public async Task GetOsVersionDataByNxosTypeAsync_Succeeds()
		{
			var versions =
				await CiscoClient
					.SecurityAdvisory
					.GetOsVersionDataByType("nxos")
				.ConfigureAwait(true);

			versions.Should().NotBeNull();
			foreach (var version in versions)
			{
				version.NosType.Should().Be("NXOS");
				version.NosVersion.Should().NotBeNullOrEmpty();
				version.PlatformName.Should().NotBeNullOrEmpty();
			}
		}

		[Fact]
		public async Task GetAll_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAllAdvisories()
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories.Count.Should().NotBe(0);
		}

		[Fact]
		public async Task GetAllByPage_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAllAdvisories(1, 1)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories.Count.Should().Be(1);
		}

		[Fact]
		public async Task GetById_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetById("cisco-sa-tms-portal-xss-AXNeVg3s")
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories.Count.Should().Be(1);
		}

		[Fact]
		public async Task GetByCustomRequestLatest1_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByCustomRequest("latest/1")
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories.Count.Should().Be(1);
		}
	}
}