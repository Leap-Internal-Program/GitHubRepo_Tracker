# Introduction

This project is a web application retrieves data from this API.

Users can view repositories sorted by updatedAt date, number of stars, or number of forks. Additionally, users can filter repositories by topic or language and sort them in ascending or descending order.

It is developed using Blazor WebAsembly.

# Installation process

1. Clone the repository using git clone repository-url
2. Navigate to the project folder and open the solution file in Visual Studio.
3. Create a json file inside \GitHubRepoTrackerFE_Blazor\wwwroot folder and name it appsettings.json

## Configuration settings(appsettings.json file)

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

  # Build
  To build the code:
  + Open GitHubRepoTrackerFE_Blazor.sln with Visual Studio
  + Update the configuration file mentioned above.
  + From the Build menu, select "Build solution"




# Deployment Instructions

When a new commit is made to the `master` branch, the Azure DevOps pipeline is triggered and deploys the changes to the app service specified in the `AZURE_FE_APP_SERVICE_NAME` pipeline variable.

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

   - name: `AZURE_FE_RESOURCE_GROUP_NAME`, value: name of resource group created in step 3.
   - name: `AZURE_SUBSCRIPTION_ID`, value: subscription ID from Azure.
   - name: `AZURE_RESOURCE_LOCATION`, value: a region closer to your users (Optional).
   - name: `AZURE_FE_APP_SERVICE_NAME`, value: A name for your app service.

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