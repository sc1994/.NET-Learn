using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Demo.Hub
{
    public class Hub : Microsoft.AspNetCore.SignalR.Hub<IHub>
    {
        private readonly HttpContext _httpContext;

        public Hub(IHttpContextAccessor hca)
        {
            _httpContext = hca.HttpContext;
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="token">接收token</param>
        /// <returns></returns>
        public async Task Send(string message, string token)
        {
            await Clients.Group(token).Send(message);
        }

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        public async Task Ping()
        {
            await Clients.Caller.Pong($"pong {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }

        /// <summary>
        /// 上线拦截
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var token = _httpContext.Request.Query["token"];
            if (!string.IsNullOrEmpty(token))
            {
                // 将token关联到connectionId
                await Groups.AddToGroupAsync(Context.ConnectionId, token);
            }
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 下线拦截
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var token = _httpContext.Request.Query["token"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, token);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
