using GitHubRepoTrackerFE_Blazor.Models;

namespace GitHubRepoTrackerFE_Blazor.Interfaces
{
   
        public interface ILanguageService
        {
            Task<List<Language>> GetAllLanguages();
        }
    
}
