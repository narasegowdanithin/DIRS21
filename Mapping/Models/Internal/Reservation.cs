namespace DIRS21.Mapping.Models.Internal
{
    /// <summary>
    /// DIRS21 internal Reservation model
    /// </summary>
    public class Reservation
    {
        public string Id { get; set; }
        public string GuestName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}