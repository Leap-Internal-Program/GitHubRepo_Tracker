using GitHubRepoTrackerFE_Blazor.Interfaces;
using GitHubRepoTrackerFE_Blazor.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace GitHubRepoTrackerFE_Blazor.Services
{
    public class RepoService : IRepoService
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IApiAuthService _apiAuthService;
      

        public RepoService(HttpClient client, IConfiguration configuration, IApiAuthService apiAuthService)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;
            _apiAuthService = apiAuthService;


        }
        public async Task<List<Repository>> GetAllRepos()
        {
            List<Repository> repos = new List<Repository>();


            var Token = await _apiAuthService.GetAccessTokenAsync();

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var pageNumber = 1;
            var pageSize = 20;


            while (true)
            {


                var builder = new UriBuilder(_client.BaseAddress + _configuration.GetValue<string>("ApiEndpoints:GetAllReposEndpoint"));

                var query = HttpUtility.ParseQueryString(builder.Query);
                query["pageNumber"] = pageNumber.ToString();
                query["pageSize"] = pageSize.ToString();
                builder.Query = query.ToString();

                var uri = builder.ToString();
                Console.WriteLine(uri);

                try
                {
                    var response = await _client.GetAsync(uri);

                    if (!response.IsSuccessStatusCode)
                    {
                        // handle error response
                        break;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    var paginatedRepos = JsonConvert.DeserializeObject<RepositoryResponse>(content);

                    if (!paginatedRepos.data.Any())
                    {
                        // no more repos available
                        break;
                    }

                    repos.AddRange(paginatedRepos.data);

                    pageNumber++;
                }
                catch (Exception ex)
                {
                    // handle exception
                    Console.WriteLine(ex.ToString());
                    break;
                }



            }

            return repos;

        }

        public IEnumerable<Repository> ReposPerLanguage(string language)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Repository> ReposPerTopic(string topic)
        {
            throw new NotImplementedException();
        }
    }
}
