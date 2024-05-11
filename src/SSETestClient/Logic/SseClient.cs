using System.Runtime.CompilerServices;

namespace SSETestClient.Logic;

public class SseClient : ISseClient
{
    public SseClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    public async IAsyncEnumerable<string> ConnectAsync(
        Uri uri,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));

        using var response = await _httpClient.SendAsync(
                                        request,
                                        HttpCompletionOption.ResponseHeadersRead,
                                        cancellationToken);

        using var body = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(body);

        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync(cancellationToken);
            if (!string.IsNullOrEmpty(line))
            {
                yield return line;
            }
        }
    }

    private readonly HttpClient _httpClient;
}