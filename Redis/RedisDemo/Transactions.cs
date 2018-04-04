using System;
using Utilities;
using System.Diagnostics;
using StackExchange.Redis;

namespace RedisDemo
{
    public class Transactions
    {
        private readonly string _key = "trankey";
        private readonly IDatabase _db = ConnectionMultiplexer.Connect(Config.RedisConnect).GetDatabase();

        public void Trans1()
        {
            bool result;
            do
            {
                var oldValue = _db.StringGet(_key);
                var newValue = oldValue.ToInt() + 1; // 累加操作
                var tran = _db.CreateTransaction();
                tran.AddCondition(Condition.StringEqual(_key, oldValue)); // 比较值（相当于WATCH操作）
                tran.StringSetAsync(_key, newValue);
                result = !tran.Execute(); // 提交
                if (result)
                {
                    Debug.WriteLine(DateTime.Now.ToString("mm:ss fff") + " ===> 在 SetString 之前值已经被修改");
                }
            } while (result);
        }

        public void Trans2()
        {
            bool result;
            do
            {
                var oldValue = _db.StringGet(_key);
                var newValue = oldValue.ToInt() + 1; // 累加操作
                var tran = _db.CreateTransaction();
                tran.AddCondition(Condition.StringEqual(_key, oldValue)); // 比较值（相当于WATCH操作）
                tran.StringSetAsync("key2", "2"); // 用来测试所有命令，是否全部被放弃掉了。
                tran.StringSetAsync(_key, newValue, when: When.Exists);
                result = !tran.Execute(); // 提交
                if (result)
                {
                    Debug.WriteLine(DateTime.Now.ToString("mm:ss fff") + " ===> 在 SetString 之前值已经被修改");
                }
            } while (result);
        }

        public void Del()
        {
            _db.KeyDelete(_key);
        }

        public int Get()
        {
            return _db.StringGet(_key).ToInt();
        }
    }
}
