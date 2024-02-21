param location string = resourceGroup().location
param appServiceName string

output uniqueAppServiceName string = appService.name

resource appService 'Microsoft.Web/sites@2023-01-01' = {
  identity: {
    type: 'SystemAssigned' // System-assigned managed identities are useful for authenticating between Azure services w/o needing to store/rotate credentials.
  }
  location: location
  name: '${appServiceName}${uniqueString(resourceGroup().id)}'
  properties: {
    httpsOnly: true // Default: false
  }
}
