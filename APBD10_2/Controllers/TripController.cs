using APBD10.DTOs;
using APBD10.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD10.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripController : ControllerBase
{
    private  readonly ITripService _tripService;
    public TripController(ITripService tripService)
    {
        _tripService = tripService;
    }
    [HttpGet]
    public async Task<ActionResult<TripDto>> GetTrips([FromQuery] int page, [FromQuery] int pageSize = 1)
    {
        try
        {
            var result = await _tripService.GetTripsAsync(page, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Błąd: {ex.Message}");
        }
    }
}