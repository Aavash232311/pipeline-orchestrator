using Microsoft.AspNetCore.Mvc;
using pipeline_orchestrator.Data;
using pipeline_orchestrator.Services;
using pipeline_orchestrator.Engines;
using pipeline_orchestrator.Model;
using Microsoft.EntityFrameworkCore;


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

    [Route("pdf-metadata")]
    [HttpPost]
    public async Task<IActionResult> GetStream(Guid postingId, IFormFile pdfFile)
    {
        if (pdfFile == null || pdfFile.Length == 0) return BadRequest("No file uploaded.");
        // let's do that pdf game here and store metadata in the database
        int maxFileSizeMb = 5;
        long fileSize = maxFileSizeMb * 1024 * 1024;

        if (pdfFile.Length > fileSize)
        {
            return BadRequest(new
            {
                message = $"File length not to exceed {maxFileSizeMb} mb"
            });
        }

        if (pdfFile.ContentType != "application/pdf")
        {
            return BadRequest(new { message = "Only PDF documents are accepted." });
        }

        var posting = await _context.posting.FirstOrDefaultAsync(x => x.Id == postingId);

        if (posting == null) return new JsonResult(BadRequest(new { detail = "Posting not found." }));


        string postingText = _localScreening.LoadChunkForLLM(posting);
        ExtractionTopic metaDataFromPdf = _localScreening.MetaData(pdfFile);
        Talent newTalent = new Talent()
        {
            Name = "Dummy Name <we will get using this interface>",
            Email = "Email <we will get using this interface>",
            Experience = metaDataFromPdf.Experience,
            Projects = metaDataFromPdf.Projects,
            ProfessionalSummary = metaDataFromPdf.Summary,
            TechnicalSkills = metaDataFromPdf.Skills,

        };

        //_context.Add(newTalent);
        //await _context.SaveChangesAsync();
        // let's retrieve the experience first 
        string candidateTextChunk = _localScreening.CatCandidateAttribute(metaDataFromPdf);
        var response = await _microservice.PostAsJsonAsync("/feature_embeddings", new
        {
            Candidate = candidateTextChunk,
            Posting = postingText
        });
        // until and unless this system does not hire me there is a false negative. Just kidding :)
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return new JsonResult(jsonResponse);
    }

}
