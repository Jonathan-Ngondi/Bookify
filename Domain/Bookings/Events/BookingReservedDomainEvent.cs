using System;
using Domain.Abstractions;

namespace Domain.Bookings;

public record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;

