using System;
using OpenEventSourcing.Events;
using SIO.Domain.Translation;

namespace SIO.Domain.Document.Events
{
    public class DocumentUploaded : Event
    {
        public TranslationType TranslationType { get; set; }
        public DocumentUploaded(Guid aggregateId, int version, TranslationType translationType) : base(aggregateId, version)
        {
            TranslationType = translationType;
        }
    }
}
