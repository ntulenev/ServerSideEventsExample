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
        int i = 0;
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            await http.Response.WriteAsync(
                $"data: Message {i++} for userId {userId} at {DateTime.Now}\n\n", ct);

            await Task.Delay(5000, ct);
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
