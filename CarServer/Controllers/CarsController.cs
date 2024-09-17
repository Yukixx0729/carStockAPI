
using CarServer.Models;
using CarServer.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarServer.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class CarsController : ControllerBase
    {
        private readonly IcarService _carService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CarsController(IcarService carService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _carService = carService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //get all the listing cars 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        //post a new car with sign in dealer
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(NewCar car)
        {
            //check if the user is signed in
            if (!_signInManager.IsSignedIn(User))
            {
                throw new UnauthorizedAccessException("User is not signed in.");
            }
            //get the current signed in user's info
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            try
            {
                var createdCar = await _carService.CreateCarAsync(car, user);
                return createdCar;
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid information.");
            }

        }
    }
}