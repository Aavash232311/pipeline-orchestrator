using Microsoft.AspNetCore.Mvc;

namespace pipeline_orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamController : ControllerBase
{

    [HttpGet]
    [Route("get-stream")]
    public IActionResult GetStream()
    {
        return new JsonResult(Ok(new { message = "Hello world" }));
    }
}
