using DIRS21.Mapping.Core.Base;
using DIRS21.Mapping.Models.Internal;
using DIRS21.Mapping.Models.External.Google;

namespace DIRS21.Mapping.Mappers.Google
{
    /// <summary>
    /// Maps DIRS21 Reservation to Google format
    /// </summary>
    public class ReservationInternalToGoogleMapper : MapperBase<Reservation, GoogleReservation>
    {
        public override string SourceType => "Model.Reservation";
        public override string TargetType => "Google.Reservation";

        protected override GoogleReservation MapInternal(Reservation source)
        {
            return new GoogleReservation
            {
                ReservationId = source.Id,
                Guest = source.GuestName,
                StartDate = source.CheckIn.ToString("yyyy-MM-dd"),
                EndDate =   source.CheckOut.ToString("yyyy-MM-dd")
            };
        }
    }
}