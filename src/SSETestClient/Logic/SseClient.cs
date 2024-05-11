using SSETestClient.Models;
using System.Runtime.CompilerServices;

namespace SSETestClient.Logic;

public class SseClient : ISseClient
{
    public SseClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    public async IAsyncEnumerable<SseEvent> ConnectAsync(
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
            if (TryParseSseEvent(line, out SseEvent eventObj))
            {
                yield return eventObj;
            }
        }
    }

    private static bool TryParseSseEvent(string? line, out SseEvent eventObj)
    {
        eventObj = null!;

        if (string.IsNullOrWhiteSpace(line))
        {
            return false;
        }

        var delimiterPos = line.IndexOf(':');
        if (delimiterPos > -1)
        {
            var key = line[..delimiterPos].Trim().ToString();
            var payload = line[(delimiterPos + 1)..].Trim().ToString();
            eventObj = new SseEvent(key, payload);
            return true;

        }

        return false;
    }

    private readonly HttpClient _httpClient;
}