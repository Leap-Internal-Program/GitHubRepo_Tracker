param location string = resourceGroup().location
param appServiceName string

resource appService 'Microsoft.Web/sites@2023-01-01' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: appServiceName
  properties: {}
}
