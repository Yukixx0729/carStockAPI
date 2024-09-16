
using System.ComponentModel.DataAnnotations.Schema;

namespace CarServer.Models.Entities
{
    public class Car
    {
        public Guid Id { get; set; }

        public required string Make { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public required int Stock { get; set; }

        [ForeignKey("UserId")]
        public required string DealerId { get; set; }
        public virtual required ApplicationUser ApplicationUser { get; set; }
    }

}