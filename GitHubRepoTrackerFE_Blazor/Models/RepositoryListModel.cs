namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class RepositoryListModel
    {
        public List<Repository>? repositories { get; set; }
        public List<Topic>? topics { get; set; }
        public List<Language>? languages { get; set; }

    }
}
