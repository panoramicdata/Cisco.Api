using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System.Security;

namespace Cisco.Api.Security;

internal abstract partial class CustomHttpClientHandler
{
	internal async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
	{
		ValidateClientCredentials();

		var accessTokenResponse = await TokenAcquisition.AcquireAsync(
			_logger,
			Options,
			GetHttpClient,
			GetAuthBody,
			(response, httpResponseMessage, attemptCount) => TokenAcquisition.HandleErrorAsync(
				_logger,
				Options,
				response,
				httpResponseMessage,
				attemptCount,
				message => LogResponseHeaders(message, true),
				cancellationToken),
			cancellationToken).ConfigureAwait(false);

		return StoreAndReturnAccessToken(accessTokenResponse);
	}

	private async Task<bool> TryRetryAsync(int attemptCount, CancellationToken cancellationToken)
	{
		if (attemptCount < Options.MaxAttemptCount)
		{
			_logger.LogWarning("Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...", attemptCount, Options.MaxAttemptCount);
			await Task.Delay(Options.RetryDelay, cancellationToken).ConfigureAwait(false);
			return true;
		}

		return false;
	}

	private void ValidateClientCredentials()
	{
		if (Options.ClientId is null)
		{
			throw new SecurityException("Options ClientId must be set");
		}

		if (Options.ClientSecret is null)
		{
			throw new SecurityException("Options ClientSecret must be set");
		}
	}

	private string StoreAndReturnAccessToken(AccessTokenResponse accessTokenResponse)
	{
		var expireInSeconds = TokenAcquisition.ExpirySeconds(_logger, accessTokenResponse);

		_accessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(expireInSeconds);

		_logger.LogDebug(
			"The access token '{AccessToken}' expiry date time is '{ExpiryDateTimeUtc}'",
			accessTokenResponse.AccessToken!,
			_accessTokenExpiryDateTimeOffset
		);

		return accessTokenResponse.AccessToken!;
	}
}