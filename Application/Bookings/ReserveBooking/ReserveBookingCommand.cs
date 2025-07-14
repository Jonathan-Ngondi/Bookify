using System;
using Application.Abstractions.Messaging;
using Domain.Abstractions;

namespace Application.Bookings.ReserveBooking;

public record ReserveBookingCommand(
    Guid ApartmentId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate) : ICommand<Guid>;

