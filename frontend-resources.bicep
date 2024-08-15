param location string = resourceGroup().location
param appServiceName string = 'gitrepofe${uniqueString(resourceGroup().id)}'
param applicationInsightsName string = 'gitrepoappinsights${uniqueString(resourceGroup().id)}'


resource workspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: applicationInsightsName
  location: location
}
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: workspace.id
  }
}

resource appService 'Microsoft.Web/sites@2023-01-01' = {
  identity: {
    type: 'SystemAssigned' // System-assigned managed identities are useful for authenticating between Azure services w/o needing to store/rotate credentials.
  }
  location: location
  name: appServiceName
  properties: {
    httpsOnly: true // Default: false
    siteConfig: {
      appSettings:[
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }  
      
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }

        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~2'
        }
      ]
      
    }
  }
}

output uniqueFEAppServiceName string = appServiceName

