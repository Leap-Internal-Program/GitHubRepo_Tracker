param location string = resourceGroup().location
param appServiceName string

output uniqueAppServiceName string = appService.name

resource appService 'Microsoft.Web/sites@2023-01-01' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: '${appServiceName}${uniqueString(resourceGroup().id)}'
  properties: {}
}
