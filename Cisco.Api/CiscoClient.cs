using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cisco.Api
{
	public partial class CiscoClient : IDisposable
	{
		private readonly string _clientId;
		private readonly string _clientSecret;
		private readonly HttpClient _httpClient;
		private string _accessToken;

		public CiscoClient(string clientId, string clientSecret)
		{
			_httpClient = new HttpClient
				{BaseAddress = new Uri("https://api.cisco.com/")};
			_clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
			_clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
		}

		public async Task AuthenticateAsync()
		{
			if (IsAuthenticated)
			{
				throw new InvalidOperationException("Already authenticated.");
			}

			using (var httpClient = new HttpClient
				{BaseAddress = new Uri("https://cloudsso.cisco.com/")})
			{
				var stringContent = new StringContent($"client_id={_clientId}&grant_type=client_credentials&client_secret={_clientSecret}", Encoding.UTF8, "application/x-www-form-urlencoded");
				var response = await httpClient.PostAsync("as/token.oauth2", stringContent);
				var contents = await response.Content.ReadAsStringAsync();
				var accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(contents);
				if (accessTokenResponse.ErrorDescription != null || accessTokenResponse.Error != null)
				{
					throw new SecurityException($"{accessTokenResponse.Error}: {accessTokenResponse.ErrorDescription}");
				}
				_accessToken = accessTokenResponse.AccessToken;
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
			}
		}

		public bool IsAuthenticated => _accessToken != null;

		public void Dispose()
		{
			_httpClient?.Dispose();
		}

		private async Task<T> GetAsync<T>(string s, CancellationToken? cancellationToken)
		{
			if(!IsAuthenticated) await AuthenticateAsync();
			var response = cancellationToken.HasValue
				? await _httpClient.GetAsync(s, cancellationToken.Value)
				: await _httpClient.GetAsync(s);
			var contents = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<T>(contents);
			return result;
		}
	}
}
