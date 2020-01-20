using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEventSourcing.Queries;
using SIO.API.V1.Document.Responses;
using SIO.Domain.Projections.UserDocument.Queries;

namespace SIO.API.V1.Document
{
    public class DocumentController : SIOController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public DocumentController(IQueryDispatcher queryDispatcher)
        {
            if (queryDispatcher == null)
                throw new ArgumentNullException(nameof(queryDispatcher));

            _queryDispatcher = queryDispatcher;
        }

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
    }
}
