

using Microsoft.AspNetCore.Identity;

namespace CarServer.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Car>? Car { get; set; }
    }
}