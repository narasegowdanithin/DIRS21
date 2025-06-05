namespace DIRS21.Mapping.Models.External.Google
{
    /// <summary>
    /// Google's Room model format
    /// </summary>
    public class GoogleRoom
    {
        public string Id { get; set; }
        public string RoomType { get; set; }
        public double Rate { get; set; }
    }
}