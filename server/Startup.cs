#define UseOptions // or NoOptions
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EchoApp
{
  public class Startup
  {
    public List<String> msgs = new List<String>(){
            "The", "main", "difference", "between",
          "altcoins", "and", "tokens", "is", "in",
          "their", "structure;", "altcoins", "are",
          "separate", "currencies", "with", "their",
          "own", "separate", "blockchain", "while",
          "tokens", "operate", "on", "top", "of",
          "a", "blockchain", "that", "facilitates",
          "the", "creation", "of", "decentralized", "applications."
        };

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(LogLevel.Debug);
      loggerFactory.AddDebug(LogLevel.Debug);

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

#if NoOptions
      #region UseWebSockets
            app.UseWebSockets();
      #endregion
#endif
#if UseOptions
      #region UseWebSocketsOptions
      var webSocketOptions = new WebSocketOptions()
      {
        KeepAliveInterval = TimeSpan.FromSeconds(120),
        ReceiveBufferSize = 4 * 1024,
      };
      app.UseWebSockets(webSocketOptions);
      #endregion
#endif
      #region AcceptWebSocket
      app.Use(async (context, next) =>
      {
        if (context.Request.Path == "/ws")
        {
          if (context.WebSockets.IsWebSocketRequest)
          {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                  //await Echo(context, webSocket);
                  //await Word(context, webSocket);
                  await PushWord(context, webSocket);
          }
          else
          {
            context.Response.StatusCode = 400;
          }
        }
        else
        {
          await next();
        }

      });
      #endregion
      app.UseFileServer();
    }
    #region Echo
    /***
     * sample of echoing the message
     * */
    private async Task Echo(HttpContext context, WebSocket webSocket)
    {
      var buffer = new byte[1024 * 4];
      WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
      while (!result.CloseStatus.HasValue)
      {
        await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
      }
      await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
    #endregion
    /***
     * returns the next word
     * */
    private async Task Word(HttpContext context, WebSocket webSocket)
    {
      var buffer = new byte[1024 * 4];
      var count = 0;
      WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
      while (!result.CloseStatus.HasValue)
      {
        await webSocket.SendAsync(Encoding.ASCII.GetBytes(this.msgs[count]), result.MessageType, result.EndOfMessage, CancellationToken.None);
        count++;
        if (count >= this.msgs.Count) { count = 0; }
        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
      }
      await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
    /***
     * pushes a new word every 2s
     * */
    private async Task PushWord(HttpContext context, WebSocket webSocket)
    {
      var count = 0;
      for (var i = 0; i < 120; i++)
      {
        await webSocket.SendAsync(Encoding.ASCII.GetBytes(this.msgs[count]), WebSocketMessageType.Text, true, CancellationToken.None);
        count++;
        if (count >= this.msgs.Count) { count = 0; }
        Thread.Sleep(2000);
      }
      //await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
  }
}