﻿using System;
using Newtonsoft.Json;
using OpenEventSourcing.Events;
using SIO.Infrastructure;

namespace SIO.Domain.Document.Events
{
    public class DocumentUploaded : Event
    {
        public TranslationType TranslationType { get; set; }
        public string TranslationSubject { get; set; }
        public string FileName { get; set; }
        public DocumentUploaded(Guid aggregateId, TranslationType translationType, string translationSubject, string fileName) : base(aggregateId, 1)
        {
            TranslationType = translationType;
            TranslationSubject = translationSubject;
            FileName = fileName;
        }

        [JsonConstructor]
        public DocumentUploaded(Guid aggregateId, Guid userId, TranslationType translationType, string translationSubject, string fileName) : base(aggregateId, 1)
        {
            TranslationType = translationType;
            TranslationSubject = translationSubject;
            FileName = fileName;
            UserId = userId.ToString();
        }
    }
}
