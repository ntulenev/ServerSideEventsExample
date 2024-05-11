# ServerSideEvents example
This project demonstrates a minimal implementation of SSE server based on asp.net

The server provides an HTTP GET endpoint "/events/for/{userId}" with the header "text/event-stream". 
Once a connection is established to this endpoint, messages begin to be transmitted with a specified delay.

![Example of usage via Browser](example.png)


The project also contains console application with simplified implementation of an SSE client that returns data via IAsyncEnumerable

![Example of usage via Browser](example2.png)
