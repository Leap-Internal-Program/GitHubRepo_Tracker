using GitHubRepoTrackerFE_Blazor.Interfaces;
using GitHubRepoTrackerFE_Blazor.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace GitHubRepoTrackerFE_Blazor.Services
{
    public class LanguageService : ILanguageService
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IApiAuthService _apiAuthService;
        public LanguageService( HttpClient httpClient, IConfiguration configuration, IApiAuthService apiAuthService)
        {
            _client = httpClient;
            _configuration = configuration;
            _apiAuthService = apiAuthService;

        }
        public async Task<List<Language>> GetAllLanguages()
        {
            var Token = await _apiAuthService.GetAccessTokenAsync();
            var languages = new List<Language>();


            _client.DefaultRequestHeaders.Clear();
            //Define request data format  
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var pageNumber = 1;
            var pageSize = 20;

            bool hasMoreLanguages = true;

            while (hasMoreLanguages)
            {
                var builder = new UriBuilder(_client.BaseAddress + _configuration.GetValue<string>("ApiEndpoints:GetAllLanguagesEndpoint"));

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

                    var responseLanguages = JsonConvert.DeserializeObject<LanguageResponse>(content);

                    if (!responseLanguages.data.Any())
                    {
                        // no more topics available
                        hasMoreLanguages = false;
                    }
                    else
                    {

                        languages.AddRange(responseLanguages.data);

                        pageNumber++;
                    }
                }
                catch (Exception ex)
                {
                    // handle exception
                    Console.WriteLine(ex);
                    break;
                }



            }


            return languages;
        }
    }
}
