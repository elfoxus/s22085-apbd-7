using Microsoft.AspNetCore.Mvc;

namespace apbd_7_s22085.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly ILogger<ClientsController> _logger;
    
    public ClientsController(ILogger<ClientsController> logger)
    {
        _logger = logger;
    }
    
    // sprawdzić, czy klient posiada wycieczki
    // jeżeli posiada co najmniej 1 przypisaną, to nie można go usunąć, zwrócić błąd
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        return Ok();
    }
}