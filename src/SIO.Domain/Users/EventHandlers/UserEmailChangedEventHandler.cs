using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Extenions;
using SIO.Domain.Users.Events;
using SIO.Infrastructure.Notifications.Hubs;

namespace SIO.Domain.Users.EventHandlers
{
    internal class UserEmailChangedEventHandler : IEventHandler<UserEmailChanged>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public UserEmailChangedEventHandler(IHubContext<NotificationHub> hubContext)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(UserEmailChanged @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
