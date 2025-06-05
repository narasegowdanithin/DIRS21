
namespace DIRS21.MappingConsoleUseCase.Models
{
    // Custom model for new partner
    public class BookingReservation
    {
        public string BookingRef { get; set; }
        public string GuestFullName { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }
    }
}
