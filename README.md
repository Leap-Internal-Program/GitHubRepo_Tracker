# Deployment Instructions

When a new commit is made to the `master` branch, the `GitRepositoryTrackerBlazor_FE` Azure DevOps pipeline is triggered and deploys the changes to the app service specified in the `AZURE_FE_APP_SERVICE_NAME` pipeline variable.

## First-time deployment

If this repo is being deployed for the first time, there are several manual steps that need to be performed in conjunction with the Azure DevOps pipeline in order to fully configure all Azure resources:

### Before first pipeline run

1. In Azure, create an App registration with the default settings. This will be used by the ADO pipeline to deploy Azure resources.
2. In the Azure subscription that these resources need to be deployed within, assign the 'Reader' role to the App registration created in step 1.
3. Create a new resource group within the Azure subscription to contain all of the resources. Although the naming doesn't affect any programmatic actions, the name `git-repository-tracker-fe-rg` is recommended.
4. Within the resource group created in step 5, assign the 'Contributor' role to the App registration created in step 1.
5. In Azure DevOps Project settings > Pipelines > Service connections, create an Azure Resource Manager service connection, using the 'Service principal (manual)' option. Along with the tenant ID, subscription ID and name, provide the client ID and secret from the App registration created in step 2. You'll need to create a new secret for this step. The name for this service connection should match the resource group name created in step 3.
6. (Optional) convert service connection created in step 4 to use Workload identity federation instead. This removes the need to update the client secret every 6 months. The easiest way to do this is to select the 'Convert' option within the service connection and then, when it fails, use the 'Authentication conversion' details provided to manually create the Federated credential.
7. Edit the pipeline and add the following variables:

   - name: `AZURE_FE_RESOURCE_GROUP_NAME`, value: name of resource group created in step 3.
   - name: `AZURE_SUBSCRIPTION_ID`, value: subscription ID from Azure.
   - name: `AZURE_RESOURCE_LOCATION`, value: `South Africa North` or a region closer to your users.
   - name: `AZURE_FE_APP_SERVICE_NAME`, value: `GitHubRepoTrackerFEBlazor`.

### During first pipeline run

1. Permit use of the service connection created in step 5.
