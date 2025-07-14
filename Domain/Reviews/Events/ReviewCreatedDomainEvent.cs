using System;
using Domain.Abstractions;

namespace Domain.Reviews.Events;

public record ReviewCreatedDomainEvent(Guid ReviewId) : IDomainEvent;

