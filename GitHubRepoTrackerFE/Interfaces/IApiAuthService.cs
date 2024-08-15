namespace GitHubRepoTrackerFE.Interfaces
{
    public interface IApiAuthService
    {
        
            Task<string> GetAccessTokenAsync();
        
    }
}
