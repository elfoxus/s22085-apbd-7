using apbd_7_s22085.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_7_s22085.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly ILogger<ClientsController> _logger;
    private readonly DatabaseContext _database;
    
    public ClientsController(ILogger<ClientsController> logger, DatabaseContext database)
    {
        _logger = logger;
        _database = database;
    }
    
    // sprawdzić, czy klient posiada wycieczki
    // jeżeli posiada co najmniej 1 przypisaną, to nie można go usunąć, zwrócić błąd
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _database.Clients.Where(c => c.IdClient == id).Include(c => c.ClientTrips).FirstOrDefaultAsync();
        if (client == null)
        {
            return NotFound();
        }
        if (client.ClientTrips.Count > 0)
        {
            return BadRequest("Client has trips assigned");
        }
        _database.Clients.Remove(client);
        await _database.SaveChangesAsync();
        return Ok("Client deleted");
    }
}