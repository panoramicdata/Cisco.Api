using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Cisco.Api.Security;

internal abstract partial class CustomFastUmbrellaHttpClientHandler
{
	internal async Task<string> GetAccessTokenAsync(int currentCredentialCurrentIndex, CancellationToken cancellationToken)
	{
		var accessTokenResponse = await TokenAcquisition.AcquireAsync(
			_logger,
			Options,
			() => GetHttpClient(currentCredentialCurrentIndex),
			GetAuthBody,
			(response, httpResponseMessage, attemptCount) => TokenAcquisition.HandleErrorAsync(
				_logger,
				Options,
				response,
				httpResponseMessage,
				attemptCount,
				message => LogResponseHeaders(currentCredentialCurrentIndex, message, true),
				cancellationToken),
			cancellationToken).ConfigureAwait(false);

		return StoreAndReturnAccessToken(currentCredentialCurrentIndex, accessTokenResponse);
	}

	private string StoreAndReturnAccessToken(int currentCredentialCurrentIndex, AccessTokenResponse accessTokenResponse)
	{
		var expireInSeconds = TokenAcquisition.ExpirySeconds(_logger, accessTokenResponse);
		var credentials = CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value;

		credentials.AccessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(expireInSeconds);

		_logger.LogDebug(
			"The access token '{AccessToken}' expiry date time is '{ExpiryDateTimeUtc}'",
			accessTokenResponse.AccessToken!,
			credentials.AccessTokenExpiryDateTimeOffset
		);

		return accessTokenResponse.AccessToken!;
	}

	private async Task EnsureAuthenticatedAsync(int currentCredentialCurrentIndex, CiscoUmbrellaCredentialsTokenTracking currentCredentialsInstance, CancellationToken cancellationToken)
	{
		if (currentCredentialsInstance.AccessTokenExpiryDateTimeOffset is not null
			&& currentCredentialsInstance.AccessTokenExpiryDateTimeOffset <= DateTimeOffset.UtcNow)
		{
			_logger.LogDebug("SendAsync(): The access token expiry date time ('{AccessTokenExpiryDateTimeOffset}') has expired - getting a new token...", currentCredentialsInstance.AccessTokenExpiryDateTimeOffset);
		}

		if (currentCredentialsInstance.AuthenticationHeaderValue is null
			|| (currentCredentialsInstance.AccessTokenExpiryDateTimeOffset is not null
				&& currentCredentialsInstance.AccessTokenExpiryDateTimeOffset <= DateTimeOffset.UtcNow))
		{
			await RefreshAccessTokenAsync(currentCredentialCurrentIndex, currentCredentialsInstance, cancellationToken).ConfigureAwait(false);
		}
	}

	private async Task RefreshAccessTokenAsync(int currentCredentialCurrentIndex, CiscoUmbrellaCredentialsTokenTracking currentCredentialsInstance, CancellationToken cancellationToken)
	{
		currentCredentialsInstance.AccessToken = await GetAccessTokenAsync(currentCredentialCurrentIndex, cancellationToken).ConfigureAwait(false);
		currentCredentialsInstance.AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", currentCredentialsInstance.AccessToken);
	}
}