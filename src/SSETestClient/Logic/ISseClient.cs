namespace SSETestClient.Logic;

public interface ISseClient
{
    IAsyncEnumerable<string> ConnectAsync(Uri uri, CancellationToken cancellationToken);
}
