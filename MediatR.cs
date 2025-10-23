using System;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using ON.SeedWork.Common.MediatR.Exceptions;
using Microsoft.Extensions.Logging;

namespace ON.SeedWork.Common.MediatR;

public sealed class MediatR : IMediatR
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<MediatR> _logger;

    public MediatR(IServiceProvider provider, ILogger<MediatR> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var commandType = command.GetType();

        if (!typeof(ICommand<TResult>).IsAssignableFrom(commandType))
        {
            _logger.LogDebug("Invalid command type: {CommandType}", commandType.FullName);
            throw new ArgumentException("Invalid command type.", nameof(command));
        }

        var resultType = typeof(TResult);
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, resultType);

        _logger.LogDebug("Resolving handler {CommandType}", commandType.FullName);

        var handler = _provider.GetService(handlerType);

        if (handler == null)
        {
            _logger.LogDebug("Handler not registered on the DI: {CommandType}", commandType.FullName);
            throw new HandlerNotRegisteredException(commandType);
        }

        var handleMethod = handlerType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public);

        if (handleMethod == null || handleMethod.GetParameters().Length != 2)
        {
            _logger.LogDebug("Handler for {CommandType} did not return a Task", commandType.FullName);
            throw new InvalidOperationException($"Handler for {commandType.Name} does not implement a valid Handle method.");
        }

        _logger.LogDebug("Invoking handler {HandlerType}", handlerType.FullName);

        if (handleMethod.Invoke(handler, [command, cancellationToken]) is not Task<TResult> task)
        {
            _logger.LogDebug("Handler method for {CommandType} did not return a Task", commandType.FullName);
            throw new InvalidOperationException($"Handler method for {commandType.Name} did not return a Task.");
        }

        if (task is not Task<TResult> typedTask)
        {
            _logger.LogDebug("Handler method for {CommandType} did not return a Task<TResult>", commandType.FullName);
            throw new InvalidOperationException($"Handler method for {commandType.Name} did not return a Task<TResult>.");
        }

        return await typedTask.ConfigureAwait(false);
    }

    public async Task<TResult> SendAsync<TResult>(IQuery<TResult> command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var commandType = command.GetType();

        if (!typeof(IQuery<TResult>).IsAssignableFrom(commandType))
        {
            _logger.LogDebug("Invalid command type: {CommandType}", commandType.FullName);
            throw new ArgumentException("Invalid command type.", nameof(command));
        }

        var resultType = typeof(TResult);
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(commandType, resultType);

        _logger.LogDebug("Resolving handler {CommandType}", commandType.FullName);

        var handler = _provider.GetService(handlerType);

        if (handler == null)
        {
            _logger.LogDebug("Handler not registered on the DI: {CommandType}", commandType.FullName);
            throw new HandlerNotRegisteredException(commandType);
        }

        var handleMethod = handlerType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public);

        if (handleMethod == null || handleMethod.GetParameters().Length != 2)
        {
            _logger.LogDebug("Handler for {CommandType} did not return a Task", commandType.FullName);
            throw new InvalidOperationException($"Handler for {commandType.Name} does not implement a valid Handle method.");
        }

        _logger.LogDebug("Invoking handler {HandlerType}", handlerType.FullName);

        if (handleMethod.Invoke(handler, [command, cancellationToken]) is not Task<TResult> task)
        {
            _logger.LogDebug("Handler method for {CommandType} did not return a Task", commandType.FullName);
            throw new InvalidOperationException($"Handler method for {commandType.Name} did not return a Task.");
        }

        if (task is not Task<TResult> typedTask)
        {
            _logger.LogDebug("Handler method for {CommandType} did not return a Task<TResult>", commandType.FullName);
            throw new InvalidOperationException($"Handler method for {commandType.Name} did not return a Task<TResult>.");
        }

        return await typedTask.ConfigureAwait(false);
    }
}
