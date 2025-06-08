using APBD10.Controllers;
using APBD10.Data;
using APBD10.DTOs;
using Microsoft.EntityFrameworkCore;

namespace APBD10.Services;

public interface ITripService
{
    Task<TripDto> GetTripsAsync(int page, int pageSize = 10);
}

public class TripService : ITripService
{
    private readonly TripContext _context;
    
    public TripService(TripContext context)
    {
        _context = context;
    }

    public async Task<TripDto> GetTripsAsync(int page, int pageSize = 10)
    {
        page = Math.Max(1, page);
        pageSize = pageSize <= 0 ? 1 : pageSize;

        var query = _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Include(t => t.IdCountries)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var trips = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDetailsDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom.ToString("yyyy-MM-dd"),
                DateTo = t.DateTo.ToString("yyyy-MM-dd"),
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDto
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientTripDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            })
            .ToListAsync();

        return new TripDto
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = trips
        };
    }
}