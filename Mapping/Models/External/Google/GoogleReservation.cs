namespace DIRS21.Mapping.Models.External.Google
{
    /// <summary>
    /// Google's Reservation model format
    /// </summary>
    public class GoogleReservation
    {
        public string ReservationId { get; set; }
        public string Guest { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}