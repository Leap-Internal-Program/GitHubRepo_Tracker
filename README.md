# Introduction

This project is a web application designed to view repository data fetched from the GitHub API. The application retrieves repositories, extracts the required fields, stores them in a SQL Server database, and then sends the repositories to the frontend for viewing.

Users can view repositories sorted by updatedAt date, number of stars, or number of forks. Additionally, users can filter repositories by topic or language and sort them in ascending or descending order based on the provided criteria.

The backend is built using ASP.NET Core Web API, while the frontend is developed
using Blazor Webassembly. Both applications will be hosted on Azure App Service.

# Installation process

1. Clone the repository using git clone repository-url
2. Navigate to the project folder and open the solution file in Visual Studio.
3. Create a json file inside \GitHubRepoTrackerFE\wwwroot folder and name it appsettings.development.json

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

"ApiBaseUrl": "your api base url",
  "ApiEndpoints": {
    "GetAllReposEndpoint": "The endpoint that returns all the repositories",
    "GetAllTopicsEndpoint": "The endpoint that returns all the topics",
    "GetAllLanguagesEndpoint": "The endpoint that returns all the languages",
    "GetAccessToken": "The endpoint that returns the access token"
  },
  "AllowedHosts": "*",
  
  "TokenUserName": "The app username",
  "TokenPassword": "The password used to get the access token"
  }
  ```

  4. Create a json file inside GitHubRepoTrackerBE project and name it appsettings.json and update it as below.
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "SQL_CONNECTION_STRING": "Server=[your SQL server name];Database=[Database name];TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=True"
  },
  "GithubSettings": {
    "GitHubAccessToken": "your_github_access_token_here"
  },
  "Jwt": {
    "Key": "your_jwt_key_here",
    "Issuer": "your_jwt_issuer_here",
    "Audience": "your_jwt_audience_here",
    "Subject": "JWT for GitRepositoryTracker"
  },
  "GitHubDataFetcherSettings": {
    "Size": "Repository size in KBs",
    "Page": "Number of pages to be returned",
    "PerPage": "Number of items per page",
    "FetchIntervalInHours": "Periodic interval in hours"
  },
  "AllowedHosts": "*",
}
```

## Setting up local SQL Database
### Installation
+ Install SQL Server 2019 or higher and [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)
+ Create a database 

### Migrations
Note: Make sure to update the sql connection string with your local database connection string.
+ Open the package manager console on you your Visual studio with your project open
+ Run ``` Add-Migration InitialCreate``` to add migration
+ Run ``` Update-Database``` to update the database



# Build
To build the code:
  + Open GitHubRepoTracker.sln with Visual Studio
  + Update the configuration file mentioned above.
  + From the Build menu, select "Build solution"
  + To run the application, press F5 or click Debug > Start Debugging.

# Build and Test

1. In Visual Studio, make sure you have the latest NuGet packages installed.
2. Set up a SQL Server database and update the connection string in the appsettings.json file accordingly.
3. Build the solution using Ctrl + Shift + B or Build > Build Solution.
4. Run the tests by navigating to Test > Run All Tests.
5. To run the application, press F5 or click Debug > Start Debugging.


# Deployment Instructions

When a new commit is made to the `master` branch, the Azure DevOps pipeline is triggered and deploys the changes to the app services specified in the pipeline variable.

## First-time deployment

If this repo is being deployed for the first time, there are several manual steps that need to be performed in conjunction with the Azure DevOps pipeline in order to fully configure all Azure resources:

### Before first pipeline run

1. In Azure, create an App registration with the default settings. This will be used by the ADO pipeline to deploy Azure resources.
2. In the Azure subscription that these resources need to be deployed within, assign the 'Reader' role to the App registration created in step 1.
3. Create a new resource group within the Azure subscription to contain all of the resources.
4. Within the resource group created in step 5, assign the 'Contributor' role to the App registration created in step 1.
5. In Azure DevOps Project settings > Pipelines > Service connections, create an Azure Resource Manager service connection, using the 'Service principal (manual)' option. Along with the tenant ID, subscription ID and name, provide the client ID and secret from the App registration created in step 1. You'll need to create a new secret for this step. The name for this service connection should match the resource group name created in step 3.
6. (Optional) convert service connection created in step 4 to use Workload identity federation instead. This removes the need to update the client secret every 6 months. The easiest way to do this is to select the 'Convert' option within the service connection and then, when it fails, use the 'Authentication conversion' details provided to manually create the Federated credential.
7. Edit the pipeline and add the following variables:

   - name: `AZURE_RESOURCE_GROUP_NAME`, value: name of resource group created in step 3.
   - name: `AZURE_SUBSCRIPTION_ID`, value: subscription ID from Azure.
   - name: `AZURE_RESOURCE_LOCATION`, value: a region closer to your users (Optional).
   - name: `AZURE_APP_REGISTRATION_CLIENT_ID`, value: the client ID for the App registration created in step 1 above.
   - name: `AZURE_APP_REGISTRATION_NAME`, value: the name for the App registration created in step 1 above.

### During first pipeline run

1. Permit use of the service connection created in step 5.

### Pipeline Variables required by the application settings.
These are the variables required by the application settings. They will be used by the task overwring the application settings.

- name: `BaseUrl` , value: The base url of your API.
- name: `GetAllReposEndpoint` , value: The API endpoint for getting all the repos.
- name: `GetAllTopicsEndpoint` , value: The API endpoint for getting all the topics.
- name: `GetAllLanguagesEndpoint` , value: The API endpoint for getting all the languages.
- name: `bearertoken` , value: The API endpoint for getting the bearer token.
- name: `Token_UserName` , value: The username of the registered user.
- name: `Token_Password` , value: The password of the registered user.
- name: `GitHubAccessToken`, value: GitHub personal access token obtained from  GitHub.
- name: `JWT_KEY` , value: a random string. You can use any random string generator to generate one.