using System;
using Domain.Abstractions;

namespace Domain.Bookings.Events;

public record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvent;

