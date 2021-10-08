using Microsoft.AspNetCore.Authorization;
using SIO.API.V1.Controllers;
using SIO.Domain.Documents.Projections;
using SIO.Infrastructure.Projections;

namespace SIO.API.V1.Documents
{
    [Authorize]
    public class DocumentProjectionController : ProjectionController<Document>
    {
        public DocumentProjectionController(IProjector<Document> projector) : base(projector)
        {
        }
    }
}
