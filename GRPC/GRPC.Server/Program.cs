using System;
using Grpc.Core;
using Utilities;
using GRPC.Server.Impl;

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
		            new ServerPort(ConfigHelper.Get("Host"), ConfigHelper.Get("Port").ToInt(), ServerCredentials.Insecure)
		        }
		    };
		    server.Start();
            Console.WriteLine("ThingsToDo server listening on port " + ConfigHelper.Get("Port").ToInt());
			Console.WriteLine("Press any key to stop the server...");
			Console.ReadKey();

		    server.ShutdownAsync().Wait();
        }
	}
}
