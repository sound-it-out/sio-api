using SIO.Infrastructure.Events;

namespace SIO.Domain.Documents.Events
{
    public class DocumentUploaded : UserEvent
    {
        public TranslationType TranslationType { get; set; }
        public string FileName { get; set; }

        public DocumentUploaded(string subject, int version, string user, TranslationType translationType, string fileName) : base(subject, version, user)
        {
            TranslationType = translationType;
            FileName = fileName;
        }
    }
}
