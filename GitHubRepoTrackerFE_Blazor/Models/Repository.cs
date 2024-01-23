using System.ComponentModel.DataAnnotations;

namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class Repository
    {
        public string repositoryName { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public Language language { get; set; }
        public Topic[] repositoryTopics { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime updatedAt { get; set; }
        public int forksCount { get; set; }
        public int stargazersCount { get; set; }
    }
}
