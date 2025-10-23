using System;
using System.Threading;
using System.Threading.Tasks;

using ON.SeedWork.Common.Extensions;

namespace ON.SeedWork.Common.MediatR;

public interface IQueryHandler <TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
    public static string GetCommandName() => typeof(TQuery).Name.ToUpperSnakeCase() ?? "";
}
