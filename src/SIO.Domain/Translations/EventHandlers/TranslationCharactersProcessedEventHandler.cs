using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Extenions;
using SIO.Domain.Translations.Events;
using SIO.Infrastructure.Notifications.Hubs;

namespace SIO.Domain.Translations.EventHandlers
{
    internal class TranslationCharactersProcessedEventHandler : IEventHandler<TranslationCharactersProcessed>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public TranslationCharactersProcessedEventHandler(IHubContext<NotificationHub> hubContext)
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
