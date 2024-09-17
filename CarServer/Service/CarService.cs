using CarServer.Data;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using CarServer.Models;
using CarServer.Models.Entities;

public interface IcarService
{
    Task<IEnumerable<Car>> GetAllCarsAsync();

    Task<Car> CreateCarAsync(NewCar car, ApplicationUser user);
}

public class CarService : IcarService
{
    private readonly ApplicationDbContext _context;

    public CarService(ApplicationDbContext context)
    {
        _context = context;

    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        return await _context.Cars.ToListAsync();
    }

    public async Task<Car> CreateCarAsync(NewCar car, ApplicationUser user)
    {

        var createdCar = new Car
        {
            Id = Guid.NewGuid(),
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            Stock = car.Stock,
            DealerId = user.Id,
            ApplicationUser = user
        };

        _context.Cars.Add(createdCar);
        await _context.SaveChangesAsync();

        return createdCar;

    }



}