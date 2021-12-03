using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIO.Api.V1.Documents.Requests;
using SIO.Api.V1.Documents.Responses;
using SIO.API.V1;
using SIO.Domain.Documents.Commands;
using SIO.Domain.Documents.Queries;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SIO.Api.V1.Documents
{
    [Authorize]
    public class DocumentController : SIOController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public DocumentController(ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher)
        {
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));
            if (queryDispatcher == null)
                throw new ArgumentNullException(nameof(queryDispatcher));

            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        [HttpPost("upload")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            await _commandDispatcher.DispatchAsync(new UploadDocumentCommand(Subject.New(), null, 1, CurrentActor, request.File, request.TranslationType, request.TranslationSubject));
            return Accepted();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<UserDocumentResponse>> GetUserDocuments(int page = 1, int pageSize = 25)
        {
            var documentsResult = await _queryDispatcher.DispatchAsync(new GetDocumentsForUserQuery(CorrelationId.New(), CurrentActor, page, pageSize));

            return documentsResult.Documents.Select(d => new UserDocumentResponse(d.DocumentId, d.FileName));
        }
    }
}
