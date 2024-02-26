using GitHubRepoTrackerFE_Blazor;
using GitHubRepoTrackerFE_Blazor.Interfaces;
using GitHubRepoTrackerFE_Blazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://gitrepobemfidahy52nqmg.azurewebsites.net/") });
builder.Services.AddSingleton<IApiAuthService, ApiAuthService>();
builder.Services.AddSingleton<IRepoService, RepoService>();
builder.Services.AddSingleton<ILanguageService, LanguageService>();
builder.Services.AddSingleton<ITopicService, TopicService>();

builder.Logging.AddApplicationInsights(
    configureTelemetryConfiguration: (config) =>
    config.ConnectionString = builder.Configuration.GetConnectionString("APPINSIGHTS_INSTRUMENTATIONKEY"),
    configureApplicationInsightsLoggerOptions: (options) => { }
    );

await builder.Build().RunAsync();
