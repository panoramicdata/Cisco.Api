using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Cisco.Api.Test
{
	internal class TestPortalConfig
	{
		internal TestPortalConfig(
			string credentialsName, ILogger logger)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../.."))
				.AddJsonFile("appsettings.json");

			var configuration = builder.Build();

			var defaultCredentialsName = configuration["DefaultCredentials"];
			credentialsName = $"{credentialsName ?? defaultCredentialsName}Credentials";
			var credentialsAppSetting = configuration[$"Credentials:{credentialsName}"];

			if (credentialsAppSetting == null)
			{
				throw new Exception($"No credentials found in appsettings.json file for {credentialsName}.");
			}

			var credentials = credentialsAppSetting.Split(';');
			if (credentials.Length != 2)
			{
				throw new Exception($"Expected to find credentials in the form ClientId;ClientSecret.  Found '{credentialsAppSetting}'");
			}

			var credentialIndex = -1;
			CiscoClient = new CiscoClient(new CiscoClientOptions
			{
				ClientId = credentials[++credentialIndex],
				ClientSecret = credentials[++credentialIndex],
			}, logger);
		}

		internal CiscoClient CiscoClient { get; }
	}
}