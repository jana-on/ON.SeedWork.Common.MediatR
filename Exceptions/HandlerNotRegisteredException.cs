namespace ON.SeedWork.Common.MediatR.Exceptions;

public class HandlerNotRegisteredException : Exception
{
    public HandlerNotRegisteredException(Type CommandType)
        : base($"The requested handler is not registered. {CommandType}") { }

}
