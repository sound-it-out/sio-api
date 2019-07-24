using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Translations.Events;
using SIO.Domain.Translations.Hubs;

namespace SIO.Domain.Translations.EventHandlers
{
    internal class TranslationGeneratedEventHandler : IEventHandler<TranslationGenerated>
    {
        private readonly IHubContext<TranslationHub> _hubContext;

        public TranslationGeneratedEventHandler(IHubContext<TranslationHub> hubContext)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(TranslationGenerated @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
