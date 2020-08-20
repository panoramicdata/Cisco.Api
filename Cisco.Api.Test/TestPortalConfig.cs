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

            TestCustomerId = configuration["TestCustomerId"];
            TestInventoryId = configuration["TestInventoryId"];
            TestSoftwareEoxId = configuration["TestSoftwareEoxId"];
            TestHardwareEoxId = configuration["TestHardwareEoxId"];
            TestFieldNoticesId1 = configuration["TestFieldNoticesId1"];
            TestFieldNoticesId2 = configuration["TestFieldNoticesId2"];
            TestPsirtId1 = configuration["TestPsirtId1"];
            TestPsirtId2 = configuration["TestPsirtId2"];
        }

        internal CiscoClient CiscoClient { get; }
        public string TestCustomerId { get; }
        public string TestInventoryId { get; }
        public string TestSoftwareEoxId { get; }
        public string TestHardwareEoxId { get; }
        public string TestFieldNoticesId1 { get; }
        public string TestFieldNoticesId2 { get; }
        public string TestPsirtId1 { get; }
        public string TestPsirtId2 { get; }
    }
}