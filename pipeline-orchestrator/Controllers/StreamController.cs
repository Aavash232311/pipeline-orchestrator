using Microsoft.AspNetCore.Mvc;
using pipeline_orchestrator.Services;
using pipeline_orchestrator.Model;


namespace pipeline_orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamController : ControllerBase
{
    private readonly REST _rest;
    private readonly HttpClient _httpClient;
    public StreamController(REST rest_client, HttpClient httpClient)
    {
        _rest = rest_client;
        _httpClient = httpClient;


        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
    }

    /* 
        Making things happen in series of pipelines. 
    */

    [Route("get-stream")]
    [HttpPost]
    public async Task<IActionResult> GetStream(Talent pool)
    {

        return new JsonResult(Ok(pool));
    }
}
