using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Serilog.Sinks.OnePageSink
{
    public class OnePageSink : ILogEventSink
    {
        private readonly IHubContext<EventHub> eventHub;
        private readonly IFormatProvider formatProvider;

        public OnePageSink(IFormatProvider formatProvider, IHubContext<EventHub> eventHub)
        {
            this.formatProvider = formatProvider;
            this.eventHub = eventHub;
        }

        public void Emit(LogEvent logEvent)
        {
            string message = logEvent.RenderMessage(formatProvider);
            eventHub.Clients.All.SendAsync("LogMessage", message, logEvent).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
