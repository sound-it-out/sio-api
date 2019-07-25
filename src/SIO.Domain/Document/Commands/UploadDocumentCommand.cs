using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using OpenEventSourcing.Commands;
using SIO.Domain.Translation;

namespace SIO.Domain.Document.Commands
{
    public class UploadDocumentCommand : Command
    {
        public IFormFile File { get; set; }
        public TranslationType TranslationType { get; set; }

        public UploadDocumentCommand(Guid aggregateId, Guid correlationId, int version, string userId, IFormFile file, TranslationType translationType) : base(aggregateId, correlationId, version, userId)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            File = file;
            TranslationType = translationType;
        }
    }
}
