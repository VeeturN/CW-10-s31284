using APBD10.Controllers;
using APBD10.Data;
using APBD10.DTOs;
using APBD10.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD10.Services;

public interface IClientService
{
    Task<bool> DeleteClientAsync(int idClient);
    Task<int> AssignClientToTripAsync(int idTrip, ClientTripAssignmentDto clientDto);
}

public class ClientService : IClientService
{
    private readonly TripContext _context;

    public ClientService(TripContext context)
    {
        _context = context;
    }
    
    public async Task<bool> DeleteClientAsync(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
            return false;

        if (client.ClientTrips != null && client.ClientTrips.Any())
            throw new InvalidOperationException("Nie da się usunąc problem z asocjacją");

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<int> AssignClientToTripAsync(int idTrip, ClientTripAssignmentDto clientDto)
    {
        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
        if (trip == null) 
            throw new InvalidOperationException("Wycieczka nie istnieje ");

        if (trip.DateFrom <= DateTime.Now)
            throw new InvalidOperationException("Juz sie zaczelo nie da sie");

        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientDto.Pesel);
        int clientId;

        if (existingClient != null)
        {
            var existingAssignment = await _context.ClientTrips
                .AnyAsync(ct => ct.IdClient == existingClient.IdClient && ct.IdTrip == idTrip);

            if (existingAssignment)
                throw new InvalidOperationException("Juz jest na tej wycieczke");

            clientId = existingClient.IdClient;
        }
        else
        {
            var newClient = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Telephone = clientDto.Telephone,
                Pesel = clientDto.Pesel
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();
            clientId = newClient.IdClient;
        }

        var clientTrip = new ClientTrip
        {
            IdClient = clientId,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = clientDto.PaymentDate
        };

        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();

        return clientId;
    }
}