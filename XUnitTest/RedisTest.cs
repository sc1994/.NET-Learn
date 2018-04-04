using Xunit;
using RedisDemo;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace XUnitTest
{
    public class RedisTest
    {
        [Fact]
        void TransTest()
        {
            var tran = new Transactions();
            ThreadPool.SetMinThreads(30, 30);
            tran.Del();
            const int count = 10;
            for (var i = 0; i < count; i++)
            {
                Parallel.Invoke(
                    tran.Trans1,
                    tran.Trans1,
                    tran.Trans1);
            }

            var result = tran.Get();
            Debug.WriteLine(result);
            Assert.Equal(result, count * 3);
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
