using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Translation.Events;
using SIO.Domain.Translation.Hubs;

namespace SIO.Domain.Translation.EventHandlers
{
    internal class TranslationSuccededEventHandler : IEventHandler<TranslationSucceded>
    {
        private readonly IHubContext<TranslationHub> _hubContext;

        public TranslationSuccededEventHandler(IHubContext<TranslationHub> hubContext)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(TranslationSucceded @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
