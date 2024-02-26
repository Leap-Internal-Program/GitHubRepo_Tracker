using GitHubRepoTrackerFE_Blazor.Interfaces;
using GitHubRepoTrackerFE_Blazor.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace GitHubRepoTrackerFE_Blazor.Services
{
    public class TopicService : ITopicService
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IApiAuthService _apiAuthService;
        private readonly ILogger<TopicService> _logger;

        public TopicService(HttpClient client, IConfiguration configuration, IApiAuthService apiAuthService,ILogger<TopicService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;
            _apiAuthService = apiAuthService;
            _logger = logger;

        }
        public async Task<List<Topic>> GetAllTopics()
        {
            var Token = await _apiAuthService.GetAccessTokenAsync();
            var topics = new List<Topic>();

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var pageNumber = 1;
            var pageSize = 20;

            bool hasMoreTopics = true;

            while (hasMoreTopics)
            {
                var builder = new UriBuilder(_client.BaseAddress + _configuration.GetValue<string>("ApiEndpoints:GetAllTopicsEndpoint"));

                var query = HttpUtility.ParseQueryString(builder.Query);
                query["pageNumber"] = pageNumber.ToString();
                query["pageSize"] = pageSize.ToString();
                builder.Query = query.ToString();
                var uri = builder.ToString();

                try
                {
                    var response = await _client.GetAsync(uri);

                    if (!response.IsSuccessStatusCode)
                    {
                        // handle error response
                        break;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    var responseTopics = JsonConvert.DeserializeObject<TopicResponse>(content);

                    if (!responseTopics.data.Any())
                    {
                        // no more topics available
                        hasMoreTopics = false;
                    }
                    else
                    {
                        topics.AddRange(responseTopics.data);

                        pageNumber++;
                    }


                }
                catch (Exception ex)
                {
                    // handle exception
                    _logger.LogError($"Error: {ex.Message}");
                    break;
                }
            }


            return topics;
        }
    }
}
