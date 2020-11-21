using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Translations.Events;
using SIO.Domain.Translations.Hubs;

namespace SIO.Domain.Translations.EventHandlers
{
    internal class TranslationQueuedEventHandler : IEventHandler<TranslationQueued>
    {
        private readonly IHubContext<TranslationHub> _hubContext;

        public TranslationQueuedEventHandler(IHubContext<TranslationHub> hubContext)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(TranslationQueued @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
