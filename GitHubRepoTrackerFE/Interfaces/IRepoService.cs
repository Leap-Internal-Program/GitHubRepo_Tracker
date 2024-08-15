using GitHubRepoTrackerFE.Models;

namespace GitHubRepoTrackerFE.Interfaces
{
    public interface IRepoService
    {
        Task<List<Repository>> GetAllRepos();
        IEnumerable<Repository> ReposPerTopic(string topic);
        IEnumerable<Repository> ReposPerLanguage(string language);
    }
}
