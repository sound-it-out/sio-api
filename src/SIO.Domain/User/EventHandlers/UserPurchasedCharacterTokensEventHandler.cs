﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.User.Events;
using SIO.Domain.User.Hubs;

namespace SIO.Domain.User.EventHandlers
{
    internal class UserPurchasedCharacterTokensEventHandler : IEventHandler<UserPurchasedCharacterTokens>
    {
        private readonly IHubContext<UserHub> _hubContext;

        public UserPurchasedCharacterTokensEventHandler(IHubContext<UserHub> hubContext)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(UserPurchasedCharacterTokens @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
