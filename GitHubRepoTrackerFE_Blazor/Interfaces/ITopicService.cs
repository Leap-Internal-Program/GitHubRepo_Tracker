using GitHubRepoTrackerFE_Blazor.Models;

namespace GitHubRepoTrackerFE_Blazor.Interfaces
{
    public interface ITopicService
    {
        Task<List<Topic>> GetAllTopics();
    }
}
