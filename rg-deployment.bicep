param location string = resourceGroup().location
param appServiceName string
param applicationInsightsName string = 'gitrepoappinsights${uniqueString(resourceGroup().id)}'
output uniqueAppServiceName string = appService.name

resource appService 'Microsoft.Web/sites@2023-01-01' = {
  identity: {
    type: 'SystemAssigned' // System-assigned managed identities are useful for authenticating between Azure services w/o needing to store/rotate credentials.
  }
  location: location
  name: '${appServiceName}${uniqueString(resourceGroup().id)}'
  properties: {
    httpsOnly: true // Default: false
    siteConfig: {
      appSettings:[
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }  
      ]
      
    }
  }
}

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
