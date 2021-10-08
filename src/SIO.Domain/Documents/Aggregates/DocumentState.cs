using SIO.Domain.Documents.Events;
using SIO.Infrastructure.Domain;
using System;

namespace SIO.Domain.Documents.Aggregates
{
    public class DocumentState : IAggregateState
    {
        public string Subject { get; set; }
        public string TranslationSubject { get; set; }
        public string User { get; set; }
        public TranslationType TranslationType { get; set; }
        public TranslationProgress Progress { get; set; }
        public string FileName { get; set; }
        public long TranslationCharactersProcessed { get; set; }
        public long TranslationCharactersTotal { get; set; }
        public int Version { get; set; }
        public bool Deleted { get; set; }

        public DocumentState() { }
        public DocumentState(DocumentState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            Subject = state.Subject;
            TranslationSubject = state.TranslationSubject;
            User = state.User;
            Progress = state.Progress;
            FileName = state.FileName;
            TranslationCharactersProcessed = state.TranslationCharactersProcessed;
            TranslationCharactersTotal = state.TranslationCharactersTotal;
            Version = state.Version;
        }
    }
}
