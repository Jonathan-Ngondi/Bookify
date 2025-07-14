using System;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IPipelineBehaviour<TRequest, TResponse>
{
    Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}

