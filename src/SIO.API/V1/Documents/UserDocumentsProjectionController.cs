using Microsoft.AspNetCore.Authorization;
using SIO.API.V1.Controllers;
using SIO.Domain.Documents.Projections;
using SIO.Infrastructure.Projections;

namespace SIO.API.V1.Documents
{
    [Authorize]
    public class UserDocumentsProjectionController : ProjectionController<UserDocuments>
    {
        public UserDocumentsProjectionController(IProjector<UserDocuments> projector) : base(projector)
        {
        }
    }
}
