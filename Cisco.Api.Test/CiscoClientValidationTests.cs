using Cisco.Api;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Exercises <see cref="CiscoClient"/>'s option validation and disposal paths, neither of which
/// needs a live service.
/// </summary>
public class CiscoClientValidationTests
{
	/// <summary>A null options argument is rejected with ArgumentNullException.</summary>
	[Fact]
	public void CiscoClient_NullOptions_Throws()
	{
		Assert.Throws<ArgumentNullException>(() => new CiscoClient(null!));
	}

	/// <summary>Options without a ClientId are rejected with ArgumentException.</summary>
	[Fact]
	public void CiscoClient_MissingClientId_Throws()
	{
		var options = new CiscoClientOptions { ClientSecret = "s" };
		var ex = Assert.Throws<ArgumentException>(() => new CiscoClient(options));
		ex.Message.Should().Contain("ClientId");
	}

	/// <summary>Options without a ClientSecret are rejected with ArgumentException.</summary>
	[Fact]
	public void CiscoClient_MissingClientSecret_Throws()
	{
		var options = new CiscoClientOptions { ClientId = "c" };
		var ex = Assert.Throws<ArgumentException>(() => new CiscoClient(options));
		ex.Message.Should().Contain("ClientSecret");
	}

	/// <summary>An empty multi-credential list is rejected with ArgumentException.</summary>
	[Fact]
	public void CiscoClient_EmptyMultiCredentials_Throws()
	{
		var options = new CiscoClientOptions
		{
			ClientCredentialsNotSupported = []
		};
		var ex = Assert.Throws<ArgumentException>(() => new CiscoClient(options));
		ex.Message.Should().Contain("credentials");
	}

	/// <summary>A client can be created with valid options and immediately disposed.</summary>
	[Fact]
	public void CiscoClient_ValidOptions_ConstructsAndDisposes()
	{
		var options = new CiscoClientOptions
		{
			ClientId = "c",
			ClientSecret = "s"
		};
		using var client = new CiscoClient(options, NullLogger.Instance);
		client.Hello.Should().NotBeNull();
		client.Pss.Should().NotBeNull();
		client.Umbrella.Should().NotBeNull();
	}

	/// <summary>Dispose is idempotent.</summary>
	[Fact]
	public void CiscoClient_DisposeTwice_DoesNotThrow()
	{
		var options = new CiscoClientOptions
		{
			ClientId = "c",
			ClientSecret = "s"
		};
		var client = new CiscoClient(options);
		client.Dispose();
		client.Dispose();
	}

	/// <summary>ClientCredentialsNotSupported with one valid entry constructs cleanly.</summary>
	[Fact]
	public void CiscoClient_MultiCredentials_Constructs()
	{
		var options = new CiscoClientOptions
		{
			ClientCredentialsNotSupported = [new CiscoClientCredentials { ClientId = "c1", ClientSecret = "s1" }]
		};
		using var client = new CiscoClient(options);
		client.Umbrella.Should().NotBeNull();
	}
}
