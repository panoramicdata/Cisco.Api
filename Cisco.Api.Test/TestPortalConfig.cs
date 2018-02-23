using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	internal class TestPortalConfig
	{
		internal TestPortalConfig(string credentialsName, ITestOutputHelper iTestOutputHelper)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");
			Configuration = builder.Build();

			var defaultCredentialsName = Configuration["DefaultCredentials"];
			credentialsName = $"{credentialsName ?? defaultCredentialsName}Credentials";
			var credentialsAppSetting = Configuration[$"Credentials:{credentialsName}"];

			if (credentialsAppSetting == null)
			{
				throw new Exception($"No credentials found in AppSettings.config file for {credentialsName}.");
			}

			var credentials = credentialsAppSetting.Split(';');
			if (credentials.Length != 2)
			{
				throw new Exception($"Expected to find credentials in the form ClientId;ClientSecret.  Found {credentialsAppSetting}");
			}

			var credentialIndex = -1;
			ClientId = credentials[++credentialIndex];
			ClientSecret = credentials[++credentialIndex];

			CiscoClient = new CiscoClient(ClientId, ClientSecret);
		}

		public static IConfigurationRoot Configuration { get; set; }

		public CiscoClient CiscoClient { get; }

		internal string ClientId { get; }

		internal string ClientSecret { get; }
	}
}