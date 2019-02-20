using System.Threading.Tasks;

namespace Demo.Hub
{
    public interface IHub
    {
        Task Send(string message);

        Task Pong(string message);
    }
}
