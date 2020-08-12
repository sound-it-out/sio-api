using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SIO.Domain.Document.Hubs
{
    [Authorize]
    internal class DocumentHub : Hub
    {
    }
}
