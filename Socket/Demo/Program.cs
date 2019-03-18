using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Demo
{
    public class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            Console.WriteLine("运行在5000端口"); // todo 
            Console.ReadLine();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseUrls("http://*:5000")
                   .UseStartup<Startup>();
    }
}
