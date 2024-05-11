namespace SSETestClient.Models;

public class SseEvent
{
    public string Key { get; }
    public string Payload { get; }

    public SseEvent(string key, string payload)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(payload);

        Key = key;
        Payload = payload;
    }

    public override string ToString() => $"{Key}: {Payload}";
}
