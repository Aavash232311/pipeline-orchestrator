using pipeline_orchestrator.Model.DTOs;
using pipeline_orchestrator.Engines;
using System.Text.Json;
using System.Text;

namespace pipeline_orchestrator.Services
{
    public class REST
    {
        private readonly IConfiguration _configuration;
        private readonly Screening _localScreening;
        private readonly HttpClient _httpClient;    
        public REST(IConfiguration config, HttpClient http_client, Screening localScreening)
        {
            _configuration = config;
            _httpClient = http_client;
            _localScreening = localScreening;
        }

        public async Task<int> RepositoryInfo(string owner, string repo)
        {
            var token = Environment.GetEnvironmentVariable("GITHUB_PAT");

            var fromDate = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-ddTHH:mm:ssZ");
            var toDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "pipeline-orchestrator");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");

            var queryObject = $@"query {{ 
                         user(login: ""{owner}"") {{ 
                         contributionsCollection(from: ""{fromDate}"", to: ""{toDate}"") {{ 
                         contributionCalendar {{ totalContributions }} 
                       }} 
                  }} 
              }}";

            var payload = new { query = queryObject };

            var response = await _httpClient.PostAsync("https://api.github.com/graphql", new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            ContributionResults? outer = JsonSerializer.Deserialize<ContributionResults>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (outer != null)
            {
                return outer.data.User.ContributionsCollection.ContributionCalendar.TotalContributions;
            }
            throw new Exception("The API endpoint RepositoryInfo is not functional. ");
        }

        public async Task<int> ExtractGitHubInfo(IFormFile pdfFile)
        {

            var extractLinks = _localScreening.ExtractPdfLinks(pdfFile);
            var githubLinks = _localScreening.ExtractGitHubLinks(extractLinks);


            return 0;

        }
    }
}
