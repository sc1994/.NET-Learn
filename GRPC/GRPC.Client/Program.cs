using System;
using Grpc.Core;

namespace GRPC.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var channel = new Channel("127.0.0.1", 50051, ChannelCredentials.Insecure);
			// 初始化客户端
			var client = new ThingsToDo.ThingsToDoClient(channel);
			// 请求参数
			var request = new HelloRequest
			{
				Name = "youself"
			};
			// 发送请求,获得响应值
			var reply = client.SayHello(request);
			// 打印...
			Console.WriteLine("ThingsToDo: " + reply.Message);
			channel.ShutdownAsync().Wait();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
