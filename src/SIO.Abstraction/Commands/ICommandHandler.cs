using System.Threading.Tasks;

namespace SIO.Abstraction.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }
}
