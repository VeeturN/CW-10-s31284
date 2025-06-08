using APBD10.DTOs;
using APBD10.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD10.Controllers;


[Controller]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    [HttpDelete("/{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        try
        {
            var result = await _clientService.DeleteClientAsync(idClient);
            if (result)
            {
                return Ok("klient usunięty.");
            }
            return NotFound("Klient nie znaleziono!.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bład: {ex.Message}");
        }
    }
    
    [HttpPost("trips/{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientTripAssignmentDto clientDto)
    {
        try
        {
            var clientId = await _clientService.AssignClientToTripAsync(idTrip, clientDto);
            return Ok(new { IdClient = clientId, Message = "Klient został dodany do wycieczki" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Błąd");
        }
    }
}