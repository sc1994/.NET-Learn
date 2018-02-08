using System;
using Grpc.Core;
using System.Threading.Tasks;

namespace GRPC.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			var server = new Grpc.Core.Server
			{
				Services =
				{
					ThingsToDo.BindService(new ThingsToDoImpl())
				},
				Ports =
				{
					new ServerPort("localhost", 50051, ServerCredentials.Insecure)
				}
			};
			server.Start();

			Console.WriteLine("ThingsToDo server listening on port " + 50051);
			Console.WriteLine("Press any key to stop the server...");
			Console.ReadKey();

			server.ShutdownAsync().Wait();
		}
	}
	
	/// <summary>
	/// 实现接口方法
	/// </summary>
	class ThingsToDoImpl : ThingsToDo.ThingsToDoBase
	{
		// Server side handler of the SayHello RPC
		public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
		{
			return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
		}
	}
}
