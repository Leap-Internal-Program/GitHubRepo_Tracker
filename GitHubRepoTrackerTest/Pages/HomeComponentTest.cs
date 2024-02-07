
using GitHubRepoTrackerFE_Blazor.Interfaces;
using GitHubRepoTrackerFE_Blazor.Models;
using GitHubRepoTrackerFE_Blazor.Pages;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GitHubRepoTrackerTest.Pages

{
    public class HomeComponentTest : TestContext
    {

        

        [Fact]
        public async Task TestServiceInjection()
        {

            //Arrange

            var mockRepoService = new Mock<IRepoService>();
            mockRepoService.Setup(service => service.GetAllRepos())
                           .ReturnsAsync(new List<Repository>());

            var mockLanguageService = new Mock<ILanguageService>();
            mockLanguageService.Setup(service => service.GetAllLanguages())
                               .ReturnsAsync(new List<Language>());

            var mockTopicService = new Mock<ITopicService>();
            mockTopicService.Setup(service => service.GetAllTopics())
                            .ReturnsAsync(new List<Topic>());

            Services.AddSingleton(mockRepoService.Object);
            Services.AddSingleton(mockLanguageService.Object);
            Services.AddSingleton(mockTopicService.Object);

            //Act

            var cut = RenderComponent<Home>();

            //Assert
            Assert.NotNull(cut.Instance.RepositoryListModel.languages);
            Assert.NotNull(cut.Instance.RepositoryListModel.topics);
            Assert.NotNull(cut.Instance.RepositoryListModel.repositories);


        }

    }
}