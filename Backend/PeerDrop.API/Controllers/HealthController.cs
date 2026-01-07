using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using PeerDrop.API.Documentations;

namespace PeerDrop.API.Controllers;

[ApiController]
[ApiVersionNeutral]
[Route("health")]
[Produces("application/json")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = HealthEndpoints.Check.Summary, Description = HealthEndpoints.Check.Description)]
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
