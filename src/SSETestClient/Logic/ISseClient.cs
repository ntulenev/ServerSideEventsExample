using SSETestClient.Models;

namespace SSETestClient.Logic;

public interface ISseClient
{
    IAsyncEnumerable<SseEvent> ConnectAsync(Uri uri, CancellationToken cancellationToken);
}
