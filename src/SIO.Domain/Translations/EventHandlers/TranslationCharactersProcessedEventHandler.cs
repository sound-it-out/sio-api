using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Translations.Events;
using SIO.Domain.Translations.Hubs;

namespace SIO.Domain.Translations.EventHandlers
{
    internal class TranslationCharactersProcessedEventHandler : IEventHandler<TranslationCharactersProcessed>
    {
        private readonly IHubContext<TranslationHub> _hubContext;

        public TranslationCharactersProcessedEventHandler(IHubContext<TranslationHub> hubContext)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(TranslationCharactersProcessed @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
