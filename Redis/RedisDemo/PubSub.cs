using System.Diagnostics;
using StackExchange.Redis;

namespace RedisDemo
{
    public class PubSub
    {
        private readonly ISubscriber _pubSub = ConnectionMultiplexer.Connect("118.24.27.231:6382").GetSubscriber();
        public void Pub(string id) // 发布
        {
            _pubSub.Publish($"this {id} id", "Hello World!");
        }

        public void Sub(string id) // 订阅
        {
            _pubSub.Subscribe($"this {id} id",
                              (channel, message) =>
                              { Debug.WriteLine($"对{id}发布了：{ message}"); });
        }
    }
}
