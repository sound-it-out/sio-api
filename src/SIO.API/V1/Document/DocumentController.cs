using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Extensions;
using OpenEventSourcing.Queries;
using SIO.API.V1.Document.Requests;
using SIO.API.V1.Document.Responses;
using SIO.Domain.Document.Commands;
using SIO.Domain.Projections.UserDocument.Queries;

namespace SIO.API.V1.Document
{
    public class DocumentController : SIOController
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public DocumentController(IQueryDispatcher queryDispatcher,
            ICommandDispatcher commandDispatcher)
        {
            if (queryDispatcher == null)
                throw new ArgumentNullException(nameof(queryDispatcher));
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDocumentResponse>> Get(int page, int pageSize)
        {
            var documentsResult = await _queryDispatcher.DispatchAsync(new GetDocumentsForUserQuery(Guid.NewGuid(), CurrentUserId.ToString(), CurrentUserId.Value, page, pageSize));

            return documentsResult.Documents.Select(d => new UserDocumentResponse
            {
                Id = d.DocumentId,
                Condition = d.Data.Condition,
                FileName = d.Data.FileName
            });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm]UploadRequest request)
        {
            await _commandDispatcher.DispatchAsync(new UploadDocumentCommand(Guid.NewGuid().ToSequentialGuid(), Guid.NewGuid(), 0, Guid.NewGuid().ToString(), request.File, request.TranslationType));

            return Accepted();
        }
    }
}
