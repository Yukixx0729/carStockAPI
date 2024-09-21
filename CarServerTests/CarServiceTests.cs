
using CarServer.Data;
using CarServer.Models;
using CarServer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CarServerTests
{
    public class CarServiceTests
    {
        private readonly ApplicationDbContext _context;

        private readonly CarService _service;

        public CarServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _service = new CarService(_context);
        }


        [Fact]
        private async Task InsertDataAsync()
        {
            var cars = new List<Car>
            {
                new(){Id = Guid.Parse("9f1c6bc3-3e90-4f58-9f68-8fb91a4f34b1"),Make = "Honda", Model="Accord", Year =2019, Stock=2, DealerId="1", ApplicationUser = new ApplicationUser{}},
                new(){Id = Guid.NewGuid(),Make = "VW", Model="Golf", Year =2020, Stock=1, DealerId="2", ApplicationUser = new ApplicationUser{}},
                new(){Id =Guid.NewGuid(),Make = "VW", Model="Tiguan", Year =2009, Stock=2, DealerId="2", ApplicationUser = new ApplicationUser{}},
            };

            var users = new List<ApplicationUser>
            {
                new(){Id = "1"},

            };

            await _context.Cars.AddRangeAsync(cars);
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        //tests
        [Fact]
        public async Task CreateCarAsync_RetuenCar()
        {
            await InsertDataAsync();
            var newCar = new NewCar
            {
                Make = "Toyota",
                Model = "Yaris",
                Year = 2024,
                Stock = 2,
            };
            var user = new ApplicationUser
            {
                Id = "2"
            };
            var createdCar = await _service.CreateCarAsync(newCar, user);
            Assert.Equal("Yaris", createdCar.Model);

            var cars = await _service.GetAllCarsByDealerIdAsync("2");

            Assert.Equal(3, cars.Count());

        }


        [Fact]
        public async Task GetCarByIdAsync_ReturnCarById()
        {
            await InsertDataAsync();

            var result = await _service.GetCarByIdAsync(Guid.Parse("9f1c6bc3-3e90-4f58-9f68-8fb91a4f34b1"));
            Assert.Equal("Accord", result.Model);
        }

        [Fact]
        public async Task GetAllCarsByDealerIdAsync_ReturnCarsByDealerId()
        {
            await InsertDataAsync();
            var results = await _service.GetAllCarsByDealerIdAsync("1");
            Assert.Single(results);
            Assert.Contains(results, c => c.Model == "Accord");

        }

        [Fact]
        public async Task UpdateCarAsync_ReturnTrue()
        {
            await InsertDataAsync();
            var updatedCar = new NewCar
            {
                Model = "Civic",
                Make = "Honda",
                Year = 2021,
                Stock = 1
            };

            var result = await _service.UpdateCarAsync(Guid.Parse("9f1c6bc3-3e90-4f58-9f68-8fb91a4f34b1"), updatedCar);
            Assert.True(result);
            var car = await _service.GetCarByIdAsync(Guid.Parse("9f1c6bc3-3e90-4f58-9f68-8fb91a4f34b1"));

            Assert.Equal("Civic", car.Model);

        }

        [Fact]
        public async Task DeleteCarAsync_ReturnTrue()
        {
            await InsertDataAsync();

            var result = await _service.DeleteCarAsync(Guid.Parse("9f1c6bc3-3e90-4f58-9f68-8fb91a4f34b1"));

            Assert.True(result);

            var deletedCar = await _context.Cars.FindAsync(Guid.Parse("9f1c6bc3-3e90-4f58-9f68-8fb91a4f34b1"));
            Assert.Null(deletedCar);

        }

        [Fact]
        public async Task GetCarsByFilterAsync_ReturnCarsByFilter()
        {
            await InsertDataAsync();

            var filter = new CarFilter
            {
                Make = "VW"
            };

            var results = await _service.GetCarsByFilterAsync(filter, "2");
            Assert.Equal(2, results.Count());
            Assert.Contains(results, c => c.Model == "Tiguan");
            Assert.Contains(results, c => c.Model == "Golf");

        }


    }

}