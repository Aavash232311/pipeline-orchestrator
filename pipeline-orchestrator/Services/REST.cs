using System.Net.Http;
using System.Net.Http.Headers;

namespace pipeline_orchestrator.Services
{
    public class REST
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;    
        public REST(IConfiguration config, HttpClient http_client)
        {
            _configuration = config;
            _httpClient = http_client;
        }

        public async Task<string> RepositoryInfo(string owner, string repo)
        {
            string url = $"https://api.github.com/repos/Aavash232311/AspNet-SignalR-Full-Stack-Project";
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Aavash232311");

            using HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
