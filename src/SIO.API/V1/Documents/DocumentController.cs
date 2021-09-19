using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIO.Api.V1.Documents.Requests;
using SIO.API.V1;
using SIO.Domain.Documents.Commands;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;
using System;
using System.Threading.Tasks;

namespace SIO.Api.V1.Documents
{
    [Authorize]
    public class DocumentController : SIOController
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public DocumentController(ICommandDispatcher commandDispatcher)
        {
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            await _commandDispatcher.DispatchAsync(new UploadDocumentCommand(Subject.New(), null, 1, CurrentActor, request.File, request.TranslationType));
            return Accepted();
        }
    }
}
