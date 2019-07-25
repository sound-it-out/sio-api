using System;
using System.Collections.Generic;
using System.Text;
using OpenEventSourcing.Domain;

namespace SIO.Domain.Document
{
    public class DocumentState : IAggregateState
    {
        public Guid? Id { get; set; }
        public DocumentCondition Condition { get; set; }
        public string FilePath { get; set; }
        public string TranslationPath { get; set; }
        public int Version { get; set; }

        public DocumentState() { }
        public DocumentState(DocumentState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            Id = state.Id;
            Condition = state.Condition;
            FilePath = state.FilePath;
            TranslationPath = state.TranslationPath;
            Version = state.Version;
        }
    }
}
