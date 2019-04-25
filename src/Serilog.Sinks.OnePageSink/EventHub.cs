using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

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
