using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIRS21.Mapping.Core.Interfaces;
using DIRS21.Mapping.Models.Internal;
using MappingConsoleUseCase.Models;

namespace MappingConsoleUseCase.Mappers
{
    public class ReservationToBookingMapper : IMapper
    {
        public string SourceType => "Model.Reservation";
        public string TargetType => "Booking.Reservation";

        public object Map(object source)
        {
            var reservation = source as Reservation;
            return new BookingReservation
            {
                BookingRef = reservation.Id,
                GuestFullName = reservation.GuestName,
                ArrivalDate = reservation.CheckIn.ToString("yyyy-MM-dd"),
                DepartureDate = reservation.CheckOut.ToString("yyyy-MM-dd")
            };
        }
    }
}
