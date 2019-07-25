using System;
using System.IO;
using OpenEventSourcing.Commands;

namespace SIO.Domain.Translation.Commands
{
    public class SaveTranslationCommand : Command
    {
        public Stream Stream { get; set; }
        public SaveTranslationCommand(Guid aggregateId, Guid correlationId, int version, string userId, Stream stream) : base(aggregateId, correlationId, version, userId)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Stream = stream;
        }
    }
}
