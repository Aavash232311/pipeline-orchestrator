using Microsoft.AspNetCore.Mvc;
using pipeline_orchestrator.Services;
using pipeline_orchestrator.Model;
using System.Text.Json;
using pipeline_orchestrator.Data;
using Microsoft.EntityFrameworkCore;
using pipeline_orchestrator.Model.Recruit;
using pipeline_orchestrator.Engines;


namespace pipeline_orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamController : ControllerBase
{
    private readonly REST _rest;
    private readonly HttpClient _httpClient;
    private readonly HttpClient _microservice;
    private readonly ApplicationDbContext _context;
    private readonly Screening _localScreening;
    public StreamController(REST rest_client, HttpClient httpClient, IHttpClientFactory factory, ApplicationDbContext context, Screening localScreening)
    {
        _rest = rest_client;
        _httpClient = httpClient;
        _microservice = factory.CreateClient("PythonPipeline"); // configured in our Program.cs

        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        _context = context;
        _localScreening = localScreening;
    }

    [Route("get-stream")]
    [HttpPost]
    public async Task<IActionResult> GetStream(Talent pool, Guid postingId)
    {
        var response = await _microservice.PostAsJsonAsync("/resume", pool);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        /*
         Calling a "python" microservice is the slowest part when scaling an application.
         This project might not work out good but it can answer a different question.
         
         .NET has the power here, so let's filter out the data from boolean attributes that we can use here.
         Only few in a pool goes to the ML model.

           
         */

        Posting? posting = await _context.posting.FindAsync(postingId);
        if (posting == null)
        {
            return new JsonResult(NotFound());
        };


        var sear = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

        return new JsonResult(Ok(sear));
    }
}
