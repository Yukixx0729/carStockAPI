namespace CarServer.Models
{
    public class CarFilter
    {
        public string? Model { get; set; }
        public string? Make { get; set; }

        public int? MinYear { get; set; }

        public int? MaxYear { get; set; }
    }
}