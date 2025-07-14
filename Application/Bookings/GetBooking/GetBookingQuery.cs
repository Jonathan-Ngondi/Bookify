using System;
using Application.Abstractions.Messages;

namespace Application.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;

