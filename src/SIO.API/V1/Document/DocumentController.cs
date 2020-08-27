using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Extensions;
using OpenEventSourcing.Queries;
using SIO.API.V1.Document.Requests;
using SIO.API.V1.Document.Responses;
using SIO.Domain.Document.Commands;
using SIO.Domain.Projections.Document.Queries;
using SIO.Domain.Projections.UserDocument.Queries;

namespace SIO.API.V1.Document
{
    [Authorize]
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
                FileName = d.Data.FileName,
                Version = d.Version
            });
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            var documentsResult = await _queryDispatcher.DispatchAsync(new DownloadByIdQuery(id, Guid.NewGuid(), CurrentUserId.ToString()));
            return File(await documentsResult.Stream, documentsResult.ContentType, $"{Path.GetFileNameWithoutExtension(documentsResult.Filename)}.mp3");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm]UploadRequest request)
        {
            await _commandDispatcher.DispatchAsync(new UploadDocumentCommand(Guid.NewGuid().ToSequentialGuid(), Guid.NewGuid(), 0, CurrentUserId.ToString(), request.File, request.TranslationOption));

            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromRoute]int version)
        {
            await _commandDispatcher.DispatchAsync(new DeleteDocumentCommand(id, Guid.NewGuid(), version, CurrentUserId.ToString()));
            return NoContent();
        }
    }
}
