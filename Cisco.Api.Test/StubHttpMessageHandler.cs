namespace Cisco.Api.Test;

/// <summary>
/// An offline message handler that records the request and answers with a canned response.
/// </summary>
internal abstract class StubHttpMessageHandler : HttpMessageHandler
{
	public HttpRequestMessage? Request { get; private set; }

	protected sealed override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		Request = request;
		var response = Respond();
		response.RequestMessage ??= request;
		return Task.FromResult(response);
	}

	protected abstract HttpResponseMessage Respond();
}