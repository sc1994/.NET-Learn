using System.Diagnostics;
using StackExchange.Redis;

namespace RedisDemo
{
    public class PubSub
    {
        private readonly ConnectionMultiplexer _multiplexer = ConnectionMultiplexer.Connect(Config.RedisConnect);
        private readonly ISubscriber _pubSub;
        public PubSub()
        {
            _multiplexer.PreserveAsyncOrder = false; // 确保消息是队列有序的
            _pubSub = _multiplexer.GetSubscriber();
        }

        public void Pub(string id) // 发布
        {
            _pubSub.Publish($"this {id} channel", "Hello World!");
        }

        public void Sub(string id) // 订阅
        {
            _pubSub.Subscribe($"this {id} channel",
                              (channel, message) =>
                              { Debug.WriteLine($"对{id}发布了：{ message}"); });
        }
    }
}
