namespace ON.SeedWork.Common.MediatR;

public interface ICommand<TResult>
{
    public bool Validate();
}
