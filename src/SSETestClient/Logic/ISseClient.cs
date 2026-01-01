using SSETestClient.Models;

namespace SSETestClient.Logic;

/// <summary>
/// Defines a client interface for connecting 
/// to a server and receiving SSE (Server-Sent Events).
/// </summary>
internal interface ISseClient
{
    /// <summary>
    /// Asynchronously connects to a server using 
    /// the specified URI and listens for SSE events.
    /// </summary>
    /// <param name="uri">The URI of the server to connect to.</param>
    /// <param name="cancellationToken">A token to monitor for 
    /// cancellation requests.</param>
    /// <returns>A stream of <see cref="SseEvent"/> instances that can be 
    /// asynchronously enumerated.</returns>
    IAsyncEnumerable<SseEvent> ConnectAsync(Uri uri, CancellationToken cancellationToken);
}