using System;
using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messages;

public interface IQuery<TResponse> :IRequest<Result<TResponse>>
{
}

