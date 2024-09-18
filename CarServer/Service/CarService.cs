using CarServer.Data;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using CarServer.Models;
using CarServer.Models.Entities;

public interface IcarService
{
    Task<IEnumerable<Car>> GetAllCarsAsync();
    Task<Car> CreateCarAsync(NewCar car, ApplicationUser user);
    Task<IEnumerable<CarDto>> GetAllCarsByDealerIdAsync(string id);
    Task<IEnumerable<CarDto>> GetCarsByFilterAsync(CarFilter filter, string id);
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

    public async Task<IEnumerable<CarDto>> GetAllCarsByDealerIdAsync(string id)
    {
        var myCars = await _context.Cars
    .Where(c => c.DealerId == id)
    .Select(c => new CarDto
    {
        Id = c.Id,
        Make = c.Make,
        Model = c.Model,
        Year = c.Year,
        Stock = c.Stock,
        DealerId = c.DealerId
    })
    .ToListAsync();

        if (myCars == null || myCars.Count == 0)
        {
            return [];
        }

        return myCars;
    }


    public async Task<IEnumerable<CarDto>> GetCarsByFilterAsync(CarFilter filter, string id)
    {
        var query = _context.Cars.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Make))
        {
            query = query.Where(c => c.Make.Contains(filter.Make));
        }
        if (!string.IsNullOrEmpty(filter.Model))
        {
            query = query.Where(c => c.Model.Contains(filter.Model));
        }
        if (filter.MinYear.HasValue)
        {
            query = query.Where(c => c.Year >= filter.MinYear);
        }
        if (filter.MaxYear.HasValue)
        {
            query = query.Where(c => c.Year <= filter.MaxYear);
        }

        var result = await query.Where(c => c.DealerId == id)
            .Select(c => new CarDto
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                Year = c.Year,
                Stock = c.Stock,
                DealerId = c.DealerId
            })
            .ToListAsync();

        return result;
    }



}