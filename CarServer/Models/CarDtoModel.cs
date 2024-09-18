namespace CarServer.Models
{
    public class CarDto
    {
        public Guid Id { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public required int Stock { get; set; }
        public required string DealerId { get; set; }
    }
}

