using BlazorApplicationInsights;
using GitHubRepoTrackerFE;
using GitHubRepoTrackerFE.Interfaces;
using GitHubRepoTrackerFE.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]) });
builder.Services.AddSingleton<IApiAuthService, ApiAuthService>();
builder.Services.AddSingleton<IRepoService, RepoService>();
builder.Services.AddSingleton<ILanguageService, LanguageService>();
builder.Services.AddSingleton<ITopicService, TopicService>();

builder.Services.AddBlazorApplicationInsights();


await builder.Build().RunAsync();
