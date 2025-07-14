using System;
using Domain.Abstractions;

namespace Domain.Bookings.Events;

public record BookingCancelledDomainEvent(Guid BookingId) : IDomainEvent;