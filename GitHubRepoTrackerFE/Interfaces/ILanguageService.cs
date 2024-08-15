using GitHubRepoTrackerFE.Models;

namespace GitHubRepoTrackerFE.Interfaces
{
   
        public interface ILanguageService
        {
            Task<List<Language>> GetAllLanguages();
        }
    
}
