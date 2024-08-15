param location string = resourceGroup().location
param appServiceName string = 'gitrepobe${uniqueString(resourceGroup().id)}'
param sqlServerName string = 'gitreposql${uniqueString(resourceGroup().id)}'
param sqlServerAdminUPN string
param sqlServerAdminObjectId string
param sqlDbName string = 'GitRepoDB'
param redisCacheName string = 'gitreporedis${uniqueString(resourceGroup().id)}'
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

resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' = {
  location: location
  name: sqlServerName
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      principalType: 'Application'
      login: sqlServerAdminUPN
      sid: sqlServerAdminObjectId
      tenantId: tenant().tenantId
      azureADOnlyAuthentication: true
    }
    minimalTlsVersion: '1.2'
  }

  resource allowAllAzureFirewallRule 'firewallRules@2021-11-01' = {
    name: 'AllowAllWindowsAzureIps'
    properties: {
      endIpAddress: '0.0.0.0'
      startIpAddress: '0.0.0.0'
    }
  }

  resource securityAlertPolicies 'securityAlertPolicies@2021-11-01' = {
    name: 'Default'
    properties: {
      state: 'Enabled'
      emailAccountAdmins: true
    }
  }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  location: location
  name: sqlDbName
  parent: sqlServer
}

resource redisCache 'Microsoft.Cache/redis@2023-08-01' = {
  location: location
  name: redisCacheName
  properties: {
    sku: {
      name: 'Basic'
      capacity: 0
      family: 'C'
    }
  }
}

output uniqueApiAppServiceName string = appServiceName
output sqlServerName string = sqlServer.properties.fullyQualifiedDomainName


resource appService 'Microsoft.Web/sites@2023-01-01' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: appServiceName
  properties: {
    httpsOnly: true
    siteConfig: {
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'REDIS_CONNECTION_STRING'
          value: '${redisCacheName}.redis.cache.windows.net:6380,password=${redisCache.listKeys().primaryKey},ssl=True,abortConnect=False'
        }
        {
          name: 'SQL_CONNECTION_STRING'
          value: 'Server=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${sqlDbName};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";'
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

       connectionStrings: [
        {
          name: 'SQL_CONNECTION_STRING'
          connectionString: 'Server=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${sqlDbName};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";'
          type: 'SQLAzure'
        }
      ]
      
    }
  }
}



