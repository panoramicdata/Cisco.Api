using System;
using System.Security;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public class AuthenticationTests : Test
	{
		public AuthenticationTests(ITestOutputHelper iTestOutputHelper)
			: base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GoodCredentials_AuthenticationSucceeds()
			=> await CiscoClient
				.AuthenticateAsync()
				.ConfigureAwait(false);

		[Fact]
		public void NoId_ThrowsException()
			=> Assert.Throws<ArgumentNullException>(() =>
				{
					var ciscoClient = new CiscoClient(null, "BAD");
				});

		[Fact]
		public void NoSecret()
			=> Assert.Throws<ArgumentNullException>(() =>
				{
					var ciscoClient = new CiscoClient("BAD", null);
				});

		[Fact]
		public async void BadCredentials()
		{
			var ciscoClient = new CiscoClient("BAD", "BAD");
			await Assert.ThrowsAsync<SecurityException>(async () => await ciscoClient.AuthenticateAsync().ConfigureAwait(false)).ConfigureAwait(false);
		}
	}
}