using apbd_7_s22085.DAL;
using apbd_7_s22085.DTOs.AddClientToTripApiModels;
using apbd_7_s22085.DTOs.GetTripApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_7_s22085.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TripsController : ControllerBase
{

    private readonly ILogger<TripsController> _logger;
    private readonly DatabaseContext _database;

    public TripsController(ILogger<TripsController> logger, DatabaseContext database)
    {
        _logger = logger;
        _database = database;
    }
    
    // lista podróży, gdzie format podróży jest w treści zadania
    // podróże mają być posortowane malejąco po dacie rozpoczęcia wycieczki
    [HttpGet(Name = "GetTrips")]
    public async Task<IActionResult> Get()
    {
        var trips = await _database.Trips
            .OrderByDescending(t => t.DateFrom)
            .Select(trip => new TripInfoDto(
                trip.Name,
                trip.Description,
                trip.DateFrom.ToString(""),
                trip.DateTo.ToString(""),
                trip.MaxPeople,
                trip.Countries.Select(country => new CountryInfoDto(country.Name)),
                trip.ClientTrips.Select(ct => new ClientInfoDto(ct.Client.FirstName, ct.Client.LastName))
            ))
            .ToListAsync();
        
        return Ok(trips);
    }
    
    // dodanie klienta do wycieczki
    // walidacja:
    // - czy klient istnieje po PESEL (jeżeli nie, to dodajemy go do bazy danych)
    // - czy klient nie jest już zapisany na podaną wycieczkę (Bad Request)
    // - czy wycieczka istnieje (Not Found)
    // paymentDate może być null, registeredAt w Client_Trip ma być ustawione na aktualną datę (Date.NOW)
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, AddClientToTripRequest request)
    {
        // let's use transaction, so that check, optional user adding and insert are atomic
        await _database.Database.BeginTransactionAsync();
        try
        {
            var client = await _database.Clients.Where(c => c.Pesel == request.Pesel).FirstOrDefaultAsync();
            if (client == null)
            {
                // client does not exist, let's add him to DB
                client = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Telephone = request.Telephone,
                    Pesel = request.Pesel
                };
                await _database.Clients.AddAsync(client);
                await _database.SaveChangesAsync();
            }
            var trip = await _database.Trips.Include(t => t.ClientTrips).Where(t => t.IdTrip == idTrip && t.Name == request.TripName).FirstOrDefaultAsync();
            if (trip == null)
            {
                return NotFound("Trip does not exist");
            }
            
            if (trip.MaxPeople <= trip.ClientTrips.Count)
            {
                return BadRequest("Trip is full");
            }
            
            var clientTrip = await _database.ClientTrips.Where(ct => ct.IdClient == client.IdClient && ct.IdTrip == trip.IdTrip).FirstOrDefaultAsync();
            if (clientTrip != null)
            {
                return BadRequest("Client already registered for this trip");
            }

            var newTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = trip.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = request.PaymentDate // might be null
            };
            await _database.ClientTrips.AddAsync(newTrip);
            await _database.SaveChangesAsync();
            await _database.Database.CommitTransactionAsync();
            return Ok($"Client with PESEL {client.Pesel} added to trip with id {idTrip}");
        }
        catch (Exception e)
        {
            await _database.Database.RollbackTransactionAsync();
            return BadRequest(e.Message);
        }
    }
}