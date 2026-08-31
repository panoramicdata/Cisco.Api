using Cisco.Api.Data.SecurityAdvisories;
using Cisco.Api.Exceptions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test
{
	/// <summary>
	/// Contains tests for security advisory operations.
	/// </summary>
	/// <param name="iTestOutputHelper">The test output helper.</param>
	public class SecurityAdvisoryTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
	{
		/// <summary>
		/// Verifies the get by type and version async succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetByTypeAndVersionAsync_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByOsTypeAndVersion("asa", "9.8.2", "", default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-asa-ssl-vpn-Y88QOm77");
		}

		/// <summary>
		/// Verifies the get by type and version with alias async succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetByTypeAndVersionWithAliasAsync_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByOsTypeAndVersion("asa", "9.8.2", "ASAV", default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-asa-ssl-vpn-Y88QOm77");
		}

		/// <summary>
		/// Verifies the get by product Cisco succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetByProductCisco_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByProduct("Cisco", default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-cucm-rce-bWNzQcUm");
		}

		/// <summary>
		/// Verifies the get by product Cisco fire power succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetByProductCiscoFirePower_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByProduct("Cisco FirePOWER Services Software for ASA", default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
		}

		/// <summary>
		/// Verifies the get by product Cisco fails scenario.
		/// </summary>
		[Fact]
		public async Task GetByProductCisco_Fails()
		{
			try
			{
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByProduct("xyz", default)
				.ConfigureAwait(true);
			}
			catch (CiscoApiException ex)
			{
				if (JsonConvert.DeserializeObject<ErrorDetailsResponse>(ex.Message)
					is ErrorDetailsResponse response)
				{
					response.ErrorCode.Should().Be("PRODUCT_NOT_FOUND");
					response.ErrorMessage.Should().Be("Product not found");
				}
			}
		}


		/// <summary>
		/// Verifies the get by CVE name async succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetByCveNameAsync_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByCveName("CVE-2022-20623", default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-nxos-bfd-dos-wGQXrzxn");
		}

		/// <summary>
		/// Verifies the get latest by ID async succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetLatestByIdAsync_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetLatesAdvisoriesLimitedByCount(15, default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories[0].AdvisoryId.Should().Be("cisco-sa-tms-portal-xss-AXNeVg3s");
		}

		/// <summary>
		/// Verifies the get OS version data by ios type async succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetOsVersionDataByIosTypeAsync_Succeeds()
		{
			var versions =
				await CiscoClient
					.SecurityAdvisory
					.GetOsVersionDataByType("ios", default)
				.ConfigureAwait(true);

			versions.Should().NotBeNull();
			foreach (var version in versions)
			{
				version.NosType.Should().Be("IOS");
				version.NosVersion.Should().NotBeNullOrEmpty();
			}
		}

		/// <summary>
		/// Verifies the get OS version data by nxos type async succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetOsVersionDataByNxosTypeAsync_Succeeds()
		{
			var versions =
				await CiscoClient
					.SecurityAdvisory
					.GetOsVersionDataByType("nxos", default)
				.ConfigureAwait(true);

			versions.Should().NotBeNull();
			foreach (var version in versions)
			{
				version.NosType.Should().Be("NXOS");
				version.NosVersion.Should().NotBeNullOrEmpty();
				version.PlatformName.Should().NotBeNullOrEmpty();
			}
		}

		/// <summary>
		/// Verifies the get all succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetAll_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAllAdvisories(true, true, default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories.Count.Should().NotBe(0);
		}

		/// <summary>
		/// Verifies the get all by page succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetAllByPage_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAllAdvisories(1, 1, true, true, default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories.Count.Should().Be(1);
		}

		/// <summary>
		/// Verifies the get by ID succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetById_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetById("cisco-sa-tms-portal-xss-AXNeVg3s", default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories.Count.Should().Be(1);
		}

		/// <summary>
		/// Verifies the get by custom request latest1 succeeds scenario.
		/// </summary>
		[Fact]
		public async Task GetByCustomRequestLatest1_Succeeds()
		{
			var securityAdvisories =
				await CiscoClient
					.SecurityAdvisory
					.GetAdvisoriesByCustomRequest("latest/1", default)
				.ConfigureAwait(true);

			securityAdvisories.Should().NotBeNull();
			securityAdvisories.Advisories.Count.Should().Be(1);
		}
	}
}