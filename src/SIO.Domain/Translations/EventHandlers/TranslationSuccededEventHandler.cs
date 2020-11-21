using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Extenions;
using SIO.Domain.Translations.Events;
using SIO.Infrastructure.Notifications.Hubs;

namespace SIO.Domain.Translations.EventHandlers
{
    internal class TranslationSuccededEventHandler : IEventHandler<TranslationSucceded>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public TranslationSuccededEventHandler(IHubContext<NotificationHub> hubContext)
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
