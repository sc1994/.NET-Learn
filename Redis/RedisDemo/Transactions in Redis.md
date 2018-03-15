Transactions in Redis
=====================

Redis中的事务和SQL类数据库中的事务有很大的区别。

Redis的事务代码块被放置在 `MULTI` 和 `EXEC` 之间，或者使用 `DISCARD` 舍弃。一旦遇到一个`MULTI` , 那么之后的命令将不会被执行，而是被排队。（且每个命令将会获得一个 `QUEUED`  ，示意这条命令被加入到队列）。 如果执行过程中遇到了 `DISCARD` ，则一切操作都会被舍弃。反之遇到 `EXEC` 则队列中的命令将被逐条提交，且逐条返回响应结果。

在SQL数据库中，您可以执行以下操作：

```C#
// assign a new unique id only if they don't already
// have one, in a transaction to ensure no thread-races
var newId = CreateNewUniqueID(); // optimistic
using(var tran = conn.BeginTran())
{
	var cust = GetCustomer(conn, custId, tran);
	var uniqueId = cust.UniqueID;
	if(uniqueId == null)
	{
		cust.UniqueId = newId;
		SaveCustomer(conn, cust, tran);
	}
	tran.Complete();
} 
```

而在Redis 的事务中值做不到保存之后进行反悔（回滚）操作。只能选择提交队列 OR 取消队列。每一个`MULTI` 和 `EXEC` 之间为一个最小单元，而且一旦开始事务， `GET` 操作将不会返回结果只会返回 `QUEUED`  直到 `EXEC` 才会得到结果

## Redis如何使用事务？

 `WATCH`和`UNWATCH`。

使用 `WATCH {key}` 将会让Redis持续监视这个值的变化，并且在这个key发生任何变化的时候，事务将被终止，队列中的命令不会被执行。需要注意的是，`DISCARD`/ `EXEC` 也会重置key 的 `WATCH ` 状态。也可以使用 `UNWATCH` 取消所有被 `WATCH` 的密钥

并发条件下实现一次 incr （伪代码）：

```
RESULT = TRUE
WHILE(RESULT)
	WATCH key1
	VAL = GET key1
	MULTI  
	SET key1 VAL + 1
	RESULT = FALSE
	EXEC  
UNWATCH
```

上面的代码大致描述了如果在我 `SET` 之前 key 被修改了，那么 `WATCH` 将会终止事务， `SET`  命令将被放弃。

事务中的 `RESULT = FALSE` 不会被执行。继续循环直到 `GET`  到 `SET`  之前没有任何改变 key1 的值的操作。

在StackExchange.Redis如何使用?
---



并发条件下实现一次 incr ：

```C#
var key = "trankey";
var result = true;
var db = ConnectionMultiplexer.Connect("118.24.27.231:6382").GetDatabase();
while (result)
{
    var oldValue = db.StringGet(key);
    var newValue = oldValue.ToInt() + 1; // 累加操作
    var tran = db.CreateTransaction();
    tran.AddCondition(Condition.StringEqual(key, oldValue)); // 比较值（相当于WATCH操作）
    tran.StringSetAsync(key, newValue, when: When.Exists); // when 限制了只有当值存在时，才会去设置。但是即使值不存到导致没有重新设置，Execute() 依然返回true。
    result = !tran.Execute(); // 提交
    // 题外话： 比如我在写主站的项目。需要写这么一段代码上去，但是我我能确定我加不加when: When.Exists返回的是true OR false的时候。在主要项目编译-运行-断点-查值，过于浪费电脑性能和时间。可以使用 c# interactive 来验证
}
```

C# 代码使用 StackExchange.Redis 实现了这段代码之上的Redis的事务，但是不同的是：没有看到关于 `WATCH` / `UNWATCH` / `MULTI` / `EXEC` / `DISCARD` 相关的代码。按照 StackExchange.Redis 的说法是：

```
This is further complicated by the fact that StackExchange.Redis uses a multiplexer approach. We can't simply let concurrent callers issue WATCH / UNWATCH / MULTI / EXEC / DISCARD: it would all be jumbled together. So an additional abstraction is provided - additionally making things simpler to get right: constraints. Constraints are basically pre-canned tests involving WATCH, some kind of test, and a check on the result. If all the constraints pass, the MULTI/EXEC is issued; otherwise UNWATCH is issued. This is all done in a way that prevents the commands being mixed together with other callers.
```

而约束条件 `tran.AddCondition(Condition.StringEqual(key, oldValue));` 基本上涉及到了 `WATCH` 。如果约束通过则执行 `EXEC` ，否则 `DISCARD` 。`AddCondition`  方法中可以设置丰富的类型验证。