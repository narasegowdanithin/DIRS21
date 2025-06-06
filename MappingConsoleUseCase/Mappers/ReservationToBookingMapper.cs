using DIRS21.Mapping.Models.Internal;
using DIRS21.MappingConsoleUseCase.Models;
using DIRS21.Mapping.Core.Base;

namespace DIRS21.MappingConsoleUseCase.Mappers
{
    public class ReservationToBookingMapper : MapperBase<Reservation,BookingReservation>
    {
        public override string SourceType => "Model.Reservation";
        public override string TargetType => "Booking.Reservation";


        /*public object Map(object source)
        {
            var reservation = source as Reservation;
            return new BookingReservation
            {
                BookingRef = reservation.Id,
                GuestFullName = reservation.GuestName,
                ArrivalDate = reservation.CheckIn.ToString("yyyy-MM-dd"),
                DepartureDate = reservation.CheckOut.ToString("yyyy-MM-dd")
            };
        }*/
      
        protected override BookingReservation MapInternal(Reservation source)
        {
            return new BookingReservation
            {
                BookingRef = source.Id,
                GuestFullName = source.GuestName,
                ArrivalDate = source.CheckIn.ToString("yyyy-MM-dd"),
                DepartureDate = source.CheckOut.ToString("yyyy-MM-dd")
            };
        }
    }
}
