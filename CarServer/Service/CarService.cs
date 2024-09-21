using CarServer.Data;
using Microsoft.EntityFrameworkCore;
using CarServer.Models;
using CarServer.Models.Entities;

public interface IcarService
{
    Task<Car> CreateCarAsync(NewCar car, ApplicationUser user);
    Task<CarDto> GetCarByIdAsync(Guid id);
    Task<IEnumerable<CarDto>> GetAllCarsByDealerIdAsync(string id);
    Task<IEnumerable<CarDto>> GetCarsByFilterAsync(CarFilter filter, string id);
    Task<bool> UpdateCarAsync(Guid id, NewCar car);
    Task<bool> DeleteCarAsync(Guid id);
}

public class CarService : IcarService
{
    private readonly ApplicationDbContext _context;

    public CarService(ApplicationDbContext context)
    {
        _context = context;

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

    public async Task<CarDto> GetCarByIdAsync(Guid id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id) ?? throw new ArgumentException("Invalid information");

        var carDto = new CarDto
        {
            Id = car.Id,
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            Stock = car.Stock,
            DealerId = car.DealerId
        };

        return carDto;
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

    public async Task<bool> UpdateCarAsync(Guid id, NewCar car)
    {
        var existingCar = await _context.Cars.FindAsync(id);

        if (existingCar == null) return false;

        existingCar.Make = car.Make;
        existingCar.Model = car.Model;
        existingCar.Year = car.Year;
        existingCar.Stock = car.Stock;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public async Task<bool> DeleteCarAsync(Guid id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null) return false;

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();

        return true;
    }

}