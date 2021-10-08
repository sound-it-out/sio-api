using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SIO.Infrastructure.Projections;

namespace SIO.API.V1.Controllers
{
    public abstract class ProjectionController<TProjection> : SIOController
        where TProjection : class, IProjection
    {
        private readonly IProjector<TProjection> _projector;

        public ProjectionController(IProjector<TProjection> projector)
        {
            if (projector == null)
                throw new ArgumentNullException(nameof(projector));

            _projector = projector;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken = default)
        {
            await _projector.StopAsync(cancellationToken);
            await _projector.ResetAsync(cancellationToken);
            await _projector.StartAsync(cancellationToken);

            return Ok();
        }
    }
}
