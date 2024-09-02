using System.Net;

namespace HttpLearning;

using ActDel = Action<HttpListenerRequest, HttpListenerResponse>;
public class TodoHttpListener
{
    private readonly HttpListener _httpListener;

    private readonly int _port;

    public event ActDel? OnGet;
    public event ActDel? OnPost;
    public event ActDel? OnPut;
    public event ActDel? OnDelete;

    
    public TodoHttpListener(int port = 80)
    {
        _httpListener = new HttpListener();
        _port = port;
        _httpListener.Prefixes.Add($"http://localhost:{_port}/");
    }

    public async Task ListenAsync()
    {
        await Task.Run(async() =>
        {
            _httpListener.Start();
            
            await Console.Out.WriteAsync($"Server is listening {_port} port...");

            while (_httpListener.IsListening)
            {
                var httpContext = await _httpListener.GetContextAsync();

                switch (httpContext.Request.HttpMethod)
                {
                    case "GET" :
                        OnGet?.Invoke(httpContext.Request, httpContext.Response);
                        break;
                    case "POST" :
                        OnPost?.Invoke(httpContext.Request, httpContext.Response);
                        break;
                    case "PUT" :
                        OnPut?.Invoke(httpContext.Request, httpContext.Response);
                        break;
                    case "DELETE" :
                        OnDelete?.Invoke(httpContext.Request, httpContext.Response);
                        break;
                }
            }
        });
    }
}