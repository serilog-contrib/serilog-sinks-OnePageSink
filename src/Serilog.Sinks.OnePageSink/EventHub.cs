using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Serilog.Sinks.OnePageSink
{
    public class EventHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("LogMessage", message);
        }
    }
}
