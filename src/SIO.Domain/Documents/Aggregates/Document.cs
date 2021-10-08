using SIO.Domain.Documents.Events;
using SIO.Infrastructure.Domain;

namespace SIO.Domain.Documents.Aggregates
{
    public class Document : Aggregate<DocumentState>
    {
        public Document(DocumentState state) : base(state)
        {
            Handles<DocumentUploaded>(Handle);
            Handles<DocumentDeleted>(Handle);
        }

        public override DocumentState GetState() => new DocumentState(_state);

        public void Upload(string subject, string user, TranslationType translationType, string fileName)
        {
            Apply(new DocumentUploaded(
                subject: subject,
                version: 1,
                user: user,
                translationType: translationType,
                fileName: fileName
            ));
        }

        public void Delete(string subject, int version)
        {
            Apply(new DocumentDeleted(
                subject: subject,
                version: Version + 1,
                user: _state.User
            ));
        }

        public void Handle(DocumentUploaded @event)
        {
            Id = @event.Subject;
            _state.Subject = @event.Subject;
            _state.TranslationType = @event.TranslationType;
            _state.FileName = @event.FileName;
            _state.User = @event.User;
            Version = @event.Version;
        }

        public void Handle(DocumentDeleted @event)
        {
            _state.Deleted = true;
            Version = @event.Version;
        }
    }
}
