using System;
using Domain.Abstractions;

namespace Domain.Bookings.Events;

public record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;

