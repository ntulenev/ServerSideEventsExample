# ServerSideEvents example
This project demonstrates a minimal implementation of SSE server based on asp.net

The server provides an HTTP GET endpoint "/events/for/{userId}" with the header "text/event-stream". 
Once a connection is established to this endpoint, messages begin to be transmitted with a specified delay.

```C#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/events/for/{userId}",
            async (HttpContext http,
                   IHostApplicationLifetime applicationLifetime,
                   int userId,
                   CancellationToken requestCt) =>
{
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(
                    requestCt,
                    applicationLifetime.ApplicationStopping);
    var ct = cts.Token;

    http.Response.Headers.Append("Cache-Control", "no-cache");
    http.Response.ContentType = "text/event-stream";
    await http.Response.StartAsync(ct);

    try
    {
        long i = 0;
        while (true)
        {
            ct.ThrowIfCancellationRequested();
            await http.Response.WriteAsync(
                $"data: Message {i++} for userId {userId} at {DateTimeOffset.Now}\n\n", ct);
            await Task.Delay(TimeSpan.FromSeconds(1), ct);
        }
    }
    catch (OperationCanceledException)
    {
    }
    finally
    {
        await http.Response.Body.FlushAsync(ct);
    }
});

app.Run();
```
![Example of usage via Browser](example.png)


The project also contains console application with simplified implementation of an SSE client that returns data via IAsyncEnumerable

```C#
using Microsoft.Extensions.DependencyInjection;

using SSETestClient.Logic;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<ISseClient, SseClient>();
serviceCollection.AddSingleton(new HttpClient());

var sp = serviceCollection.BuildServiceProvider();

using var scope = sp.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<ISseClient>();

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true;
    cts.Cancel();
    Console.WriteLine("Cancellation requested. Shutting down...");
};

var uriPath = "http://localhost:5253/events/for/123";

await foreach (var item in service.ConnectAsync(new Uri(uriPath), cts.Token))
{
    Console.WriteLine(item);
}
```

![Example of usage via Browser](example2.png)
