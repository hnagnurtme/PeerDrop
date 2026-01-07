using Microsoft.AspNetCore.Mvc;

namespace PeerDrop.API.Controllers;

[ApiController]
[ApiVersionNeutral]
[Route("health")]
[Produces("application/json")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            service = "PeerDrop API",
            version = "1.0"
        });
    }
}
