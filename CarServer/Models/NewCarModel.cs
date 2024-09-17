namespace CarServer.Models
{
    public class NewCar
    {

        public required string Make { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public required int Stock { get; set; }
    }
}