using DIRS21.Mapping.Core.Interfaces;
using DIRS21.Mapping.Models.Internal;
using DIRS21.MappingConsoleUseCase.Models;

namespace DIRS21.MappingConsoleUseCase.Mappers
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
