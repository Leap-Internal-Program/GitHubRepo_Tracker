using GitHubRepoTrackerFE.Models;

namespace GitHubRepoTrackerFE.Interfaces
{
    public interface ITopicService
    {
        Task<List<Topic>> GetAllTopics();
    }
}
