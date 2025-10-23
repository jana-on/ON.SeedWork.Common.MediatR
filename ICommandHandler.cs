using ON.SeedWork.Common.Extensions;

namespace ON.SeedWork.Common.MediatR;

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    public Task<TResult> Handle(TCommand command, CancellationToken cancellationToken = default);
    public static string GetCommandName() => typeof(TCommand).Name.ToUpperSnakeCase() ?? "";
}
