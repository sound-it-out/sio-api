using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.API.Extensions;

namespace SIO.API.V1
{
    public class SubjectUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.UserId()?.ToString() ?? "";
        }
    }
}
