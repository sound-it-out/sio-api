using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Translations.Events;
using SIO.Domain.Translations.Hubs;

namespace SIO.Domain.Translations.EventHandlers
{
    internal class TranslationStartedEventHandler : IEventHandler<TranslationStarted>
    {
        private readonly IHubContext<TranslationHub> _hubContext;

        public TranslationStartedEventHandler(IHubContext<TranslationHub> hubContext, ICommandDispatcher commandDispatcher)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(TranslationStarted @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
