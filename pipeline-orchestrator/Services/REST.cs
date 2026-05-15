using pipeline_orchestrator.Engines;
using pipeline_orchestrator.Model.DTOs;
using System.Text;
using System.Text.Json;

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

            var token = Environment.GetEnvironmentVariable("GITHUB_PAT");

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "pipeline-orchestrator");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        }

        public async Task<int> ContributionInfo(string owner, string repo)
        {

            var fromDate = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-ddTHH:mm:ssZ");
            var toDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var queryObject = $@"query {{ 
                         user(login: ""{owner}"") {{ 
                         contributionsCollection(from: ""{fromDate}"", to: ""{toDate}"") {{ 
                         contributionCalendar {{ totalContributions }} 
                       }} 
                  }} 
              }}";

            var payload = new { query = queryObject };

            try
            {
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
            } catch(Exception e)
            {
                return -1; // Here if the value is negative we won't simply count those values. We don't want some random text input to break the backend.
            }
        }

        // retive information about GitHub repo
        public async Task<int> GitHubRepoInformation(List<List<string>> nameRepo)
        {
            // todo: send a HTTP request to graph SQL and extract relivent information
            // goal is to learn about scaling, and everything possible even if this project is too broad.
            // I can handle broad things.
            return 1;
        }

        // In this method we are "trying" to extract github username, and repo name. 
       // Trying because real world is messay, and to deal with that mesh we need to think through.
        private List<List<string>> ExtractGitHubInfo(IFormFile pdfFile)
        { 
            var extractLinks = _localScreening.ExtractPdfLinks(pdfFile);
            List<string> githubLinks = _localScreening.ExtractGitHubLinks(extractLinks);

            // we want the link containing github.
            List<List<string>> ll = new List<List<string>>();

            foreach (string link in githubLinks)
            {
                Uri uri = new Uri(link);

                string hostName = uri.Host.ToLower();

                if (hostName is "github.com")
                {
                    string absPath = uri.AbsolutePath;

                    // So listen to my logic, if the hostName is github.com and two arguement is there then one is username other one is repo name in a resume.
                    // We do need to test these in lot of edge cases as well. 
                    // if this part extracts something gross, then GraphQL API method -1 which wont be counted.
                    // But we need to prevent grosh thing from going to thid party API because they have rate limiting as well.

                    string[] splittedRepos = absPath.Split('/', StringSplitOptions.RemoveEmptyEntries);


                    if (splittedRepos.Length > 1) // meaning if it's a github url with username and repo
                    {
                        ll.Add(new List<string>() { splittedRepos[0], splittedRepos[1] });
                    }
                }
            }
            return ll;
        }



        public List<List<string>> RetrivePipeline(IFormFile pdfFile)
        {
            
            List<List<string>> githubRepoUsernamePasss = ExtractGitHubInfo(pdfFile);

            var RepoInfo = GitHubRepoInformation(githubRepoUsernamePasss);
            return githubRepoUsernamePasss;
        }
    }
}
