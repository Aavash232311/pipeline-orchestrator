using Microsoft.AspNetCore.Mvc;
using pipeline_orchestrator.Services;


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

    [HttpGet]
    [Route("get-stream")]
    public async Task<IActionResult> GetStream()
    {
        var result = await _rest.RepositoryInfo("Aavash232311", "transformer-pt-analysis");
        // here we need to draw meaningful conclusion from minimum resources possible
        return new JsonResult(Ok(result));
    }
}
