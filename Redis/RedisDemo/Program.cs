using System;
using Utilities;
using StackExchange.Redis;

namespace RedisDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            new RedisTransactions().Trans2();
            Console.WriteLine("Hello World!");
        }
    }

    class RedisTransactions
    {
        private readonly string _key = "trankey";
        private bool _result = true;
        private readonly IDatabase _db = ConnectionMultiplexer.Connect("118.24.27.231:6382").GetDatabase();

        public void Trans1()
        {
            while (_result)
            {
                var oldValue = _db.StringGet(_key);
                var newValue = oldValue.ToInt() + 1; // 累加操作
                var tran = _db.CreateTransaction();
                tran.AddCondition(Condition.KeyExists(_key)); // 值必须要存在
                tran.AddCondition(Condition.StringEqual(_key, oldValue)); // 比较值（相当于WATCH操作）
                tran.StringSetAsync(_key, newValue);
                _result = !tran.Execute(); // 提交
            }
        }

        public void Trans2()
        {
            while (_result)
            {
                var oldValue = _db.StringGet(_key);
                var newValue = oldValue.ToInt() + 1; // 累加操作
                var tran = _db.CreateTransaction();
                tran.AddCondition(Condition.StringEqual(_key, oldValue)); // 比较值（相当于WATCH操作）
                tran.StringSetAsync(_key, newValue, when: When.Exists);
                _result = !tran.Execute(); // 提交
            }
        }
    }
}
