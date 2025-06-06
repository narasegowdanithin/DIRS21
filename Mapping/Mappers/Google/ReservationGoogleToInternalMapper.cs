using DIRS21.Mapping.Core.Base;
using DIRS21.Mapping.Models.Internal;
using DIRS21.Mapping.Models.External.Google;

namespace DIRS21.Mapping.Mappers.Google
{
    /// <summary>
    /// Maps Google Reservation to DIRS21 format
    /// </summary>
    public class ReservationGoogleToInternalMapper : MapperBase<GoogleReservation,Reservation>
    {
        public override string SourceType => "Google.Reservation";
        public override string TargetType => "Model.Reservation";

        protected override Reservation MapInternal(GoogleReservation source)
        {
            return new Reservation
            {
                Id = source.ReservationId,
                GuestName = source.Guest,
                CheckIn = DateTime.Parse(source.StartDate),
                CheckOut= DateTime.Parse(source.EndDate)
            };
        }
    }
}