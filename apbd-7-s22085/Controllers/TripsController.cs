using apbd_7_s22085.DTOs.AddClientToTripApiModels;
using apbd_7_s22085.DTOs.GetTripApiModels;
using Microsoft.AspNetCore.Mvc;

namespace apbd_7_s22085.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TripsController : ControllerBase
{

    private readonly ILogger<TripsController> _logger;

    public TripsController(ILogger<TripsController> logger)
    {
        _logger = logger;
    }
    
    // lista podróży, gdzie format podróży jest w treści zadania
    // podróże mają być posortowane malejąco po dacie rozpoczęcia wycieczki
    [HttpGet(Name = "GetTrips")]
    public async Task<IActionResult> Get()
    {
        return Ok(new List<TripInfoDto>());
    }
    
    // dodanie klienta do wycieczki
    // walidacja:
    // - czy klient istnieje po PESEL (NotFound)
    // - czy klient nie jest już zapisany na podaną wycieczkę (Bad Request)
    // - czy wycieczka istnieje (Not Found)
    // paymentDate może być null, registeredAt w Client_Trip ma być ustawione na aktualną datę (Date.NOW)
    [HttpPost("{id}/clients")]
    public async Task<IActionResult> AddClientToTrip(int id, AddClientToTripRequest request)
    {
        return Ok();
    }
}