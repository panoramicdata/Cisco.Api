using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Cisco.Api.Test;

internal sealed class TestPortalConfig
{
	internal TestPortalConfig(
		 string? credentialsName,
		 ILogger logger
	)
	{
		var builder = new ConfigurationBuilder()
			 .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../.."))
			 .AddJsonFile("appsettings.json");

		var configuration = builder.Build();

		var defaultCredentialsName = configuration["DefaultCredentials"];
		credentialsName = $"{credentialsName ?? defaultCredentialsName}";
		var credentialsAppSetting = configuration[$"Credentials:{credentialsName}"]
			?? throw new InvalidOperationException($"No credentials found in appsettings.json file for {credentialsName}.");

		// If credentialsAppSetting is a list of credentials, then stop
		// and throw an exception.

		var credentials = credentialsAppSetting.Split(';');
		if (credentials.Length != 2 && credentials.Length != 4) // The 4 is for Umbrella tests
		{
			throw new InvalidOperationException($"Expected to find credentials in the form ClientId;ClientSecret.  Found '{credentialsAppSetting}'");
		}

		var credentialIndex = -1;

		// Normally, takes just a single client id and secret
		// but if there are four, then it's to support the Umbrella API key pool to circumvent rate limiting
		if (credentials.Length == 2)
		{
			CiscoClient = new CiscoClient(new CiscoClientOptions
			{
				ClientId = credentials[++credentialIndex],
				ClientSecret = credentials[++credentialIndex],
				MaxAttemptCount = 100
			}, logger);
		}
		else
		{
			// Must be the Umbrella 'fast' client
			// You can use X number of client id and secret pairs to circumvent the heavy rate limiting
			CiscoClient = new CiscoClient(new CiscoClientOptions
			{
				ClientCredentialsNotSupported = [
					new CiscoClientCredentials
					{
						ClientId = credentials[++credentialIndex],
						ClientSecret = credentials[++credentialIndex]
					},
					new CiscoClientCredentials
					{
						ClientId = credentials[++credentialIndex],
						ClientSecret = credentials[++credentialIndex]
					},
				],
				MaxAttemptCount = 100
			}, logger);
		}


		TestCustomerId = GetProperty(configuration, "TestCustomerId");
		TestInventoryId = GetProperty(configuration, "TestInventoryId");
		TestDeviceId = GetProperty(configuration, "TestDeviceId");
		TestDeviceId2 = GetProperty(configuration, "TestDeviceId2");
		TestDeviceId3 = GetProperty(configuration, "TestDeviceId3");
		TestDeviceId4 = GetProperty(configuration, "TestDeviceId4");
		TestDeviceId5 = GetProperty(configuration, "TestDeviceId5");
		TestSoftwareEoxId = GetProperty(configuration, "TestSoftwareEoxId");
		TestHardwareEoxId = GetProperty(configuration, "TestHardwareEoxId");
		TestFieldNoticesId1 = GetProperty(configuration, "TestFieldNoticesId1");
		TestFieldNoticesId2 = GetProperty(configuration, "TestFieldNoticesId2");
		TestPsirtId1 = GetProperty(configuration, "TestPsirtId1");
		TestPsirtId2 = GetProperty(configuration, "TestPsirtId2");
	}

	private static string GetProperty(IConfigurationRoot configuration, string key)
	{
		return configuration[key] ?? throw new FormatException($"Missing configuration: '{key}'");
	}

	internal CiscoClient CiscoClient { get; }

	public string TestCustomerId { get; }

	public string TestInventoryId { get; }

	public string TestDeviceId { get; }

	public string TestDeviceId2 { get; }

	public string TestDeviceId3 { get; }

	public string TestDeviceId4 { get; }

	public string TestDeviceId5 { get; }

	public string TestSoftwareEoxId { get; }

	public string TestHardwareEoxId { get; }

	public string TestFieldNoticesId1 { get; }

	public string TestFieldNoticesId2 { get; }

	public string TestPsirtId1 { get; }

	public string TestPsirtId2 { get; }
}