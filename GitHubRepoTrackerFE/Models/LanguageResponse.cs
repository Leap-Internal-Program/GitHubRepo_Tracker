namespace GitHubRepoTrackerFE.Models
{
    public class LanguageResponse
    {
        public Language[] data { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }
}
