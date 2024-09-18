
using CarServer.Models;
using CarServer.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

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

        private async Task<ApplicationUser> GetSignedInUserAsync()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                throw new UnauthorizedAccessException("User is not signed in.");
            }

            var user = await _userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException("User not found.");
            return user;
        }

        //get all the listing cars 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        //get all the listing cars under the dealer id
        [HttpGet("dealer")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarsById()
        {
            //check if the user is signed in
            var user = await GetSignedInUserAsync();
            try
            {
                var carList = await _carService.GetAllCarsByDealerIdAsync(user.Id);
                return Ok(carList);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid information.");
            }
        }

        //get the cars by filters under sign in dealer
        [HttpGet("filter")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetCarsByFilter([FromQuery] CarFilter filter)
        {
            var user = await GetSignedInUserAsync();
            try
            {
                var filteredCars = await _carService.GetCarsByFilterAsync(filter, user.Id);
                return Ok(filteredCars);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid information.");
            }

        }

        //post a new car with sign in dealer
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(NewCar car)
        {
            var user = await GetSignedInUserAsync();
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