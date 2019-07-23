using System;
using System.Threading.Tasks;
using SIO.Abstraction.Commands;
using SIO.Domain.Translation.Commands;

namespace SIO.Domain.Translation.CommandHandlers
{
    internal class CreateTranslationCommandHandler : ICommandHandler<CreateTranslationCommand>
    {
        public Task ExecuteAsync(CreateTranslationCommand command)
        {
            // TODO(Matt): 
            // 1. Write file to blob storage
            // 2. Create DB entry
            throw new NotImplementedException();
        }
    }
}
