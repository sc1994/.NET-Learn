using System.Diagnostics;
using RedisDemo;
using Xunit;

namespace XUnitTest
{
    public class RedisTest
    {
        [Fact]
        void TransTest()
        {
            var tran = new Transactions();
            for (var i = 0; i < 5; i++)
            {
                System.Threading.Tasks.Parallel.Invoke(
                    tran.Trans1,
                    tran.Trans1);
            }
        }

        [Fact]
        void PubSubTest()
        {
            var pubSub = new PubSub();
            pubSub.Sub("No.1");
            pubSub.Sub("No.2");
            pubSub.Sub("No.3");
            pubSub.Sub("No.3");
            pubSub.Sub("No.3");
            Debug.WriteLine("订阅成功");
            pubSub.Pub("No.3");
        }
    }
}
