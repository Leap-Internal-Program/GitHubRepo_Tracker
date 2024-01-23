using GitHubRepoTrackerFE_Blazor.Models;

namespace GitHubRepoTrackerFE_Blazor.Interfaces
{
    public interface IRepoService
    {
        Task<List<Repository>> GetAllRepos();
        IEnumerable<Repository> ReposPerTopic(string topic);
        IEnumerable<Repository> ReposPerLanguage(string language);
    }
}
