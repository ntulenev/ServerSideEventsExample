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
