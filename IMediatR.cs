using System;
using System.Threading;
using System.Threading.Tasks;

namespace ON.SeedWork.Common.MediatR;

public interface IMediatR
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    public Task<TResult> SendAsync<TResult>(IQuery<TResult> command, CancellationToken cancellationToken = default);
}
