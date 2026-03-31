using Microsoft.AspNetCore.Mvc;
using pipeline_orchestrator.Services;
using pipeline_orchestrator.Model;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Text;
using pipeline_orchestrator.Data;


namespace pipeline_orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamController : ControllerBase
{
    private readonly REST _rest;
    private readonly HttpClient _httpClient;
    private readonly HttpClient _microservice;
    private readonly ApplicationDbContext _context;
    public StreamController(REST rest_client, HttpClient httpClient, IHttpClientFactory factory, ApplicationDbContext context)
    {
        _rest = rest_client;
        _httpClient = httpClient;
        _microservice = factory.CreateClient("PythonPipeline"); // configured in our Program.cs

        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        _context = context;
    }

    [Route("get-stream")]
    [HttpPost]
    public async Task<IActionResult> GetStream(Talent pool)
    {
        var response = await _microservice.PostAsJsonAsync("/resume", pool);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var sear = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
        return new JsonResult(Ok(sear));
    }
}
