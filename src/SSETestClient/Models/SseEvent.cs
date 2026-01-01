namespace SSETestClient.Models;

/// <summary>
/// Represents an SSE (Server-Sent Events) message.
/// </summary>
internal sealed class SseEvent
{
    /// <summary>
    /// Gets the key identifier for the SSE event.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the payload of the SSE event.
    /// </summary>
    public string Payload { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SseEvent"/> class.
    /// </summary>
    /// <param name="key">The key identifier of the event.</param>
    /// <param name="payload">The data payload of the event.</param>
    /// <exception cref="ArgumentNullException">Thrown when 
    /// <paramref name="key"/> or <paramref name="payload"/> 
    /// is null or whitespace.</exception>
    public SseEvent(string key, string payload)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);

        Key = key;
        Payload = payload;
    }

    /// <summary>
    /// Returns a string that represents the current SSE event.
    /// </summary>
    /// <returns>A string in the format "Key: Payload".</returns>
    public override string ToString() => $"{Key}: {Payload}";
}

