using Grpc.Core;
using System.Threading.Tasks;

namespace GRPC.Server.Impl
{
    public class ThingsToDoImpl : ThingsToDo.ThingsToDoBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }
    }
}
