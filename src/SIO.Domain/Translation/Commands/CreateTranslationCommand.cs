using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using SIO.Abstraction.Commands;

namespace SIO.Domain.Translation.Commands
{
    public class CreateTranslationCommand : ICommand
    {
        public Guid AggregateId { get; }
        public Guid? CorrelationId { get; }
        public IFormFile File { get; set; }

        public CreateTranslationCommand(Guid? correlationId, IFormFile file)
        {
            AggregateId = Guid.NewGuid();
            CorrelationId = correlationId;
            File = file;
        }
    }
}
