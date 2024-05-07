# BSS Framework Configurator


## Usage

Add nuget package

```xml

  <ItemGroup>
    <PackageReference Include="Luxoft.Framework.Configurator" Version="[LAST VERSION]"/>
  </ItemGroup>
```

Change `Startup.cs`

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddConfigurator();
}

public void Configure(IApplicationBuilder app)
{
    app.UseConfigurator();
}

```
Open link https://localhost:5001/admin/configurator


## Integration

Implement interface `Framework.Configurator.Interfaces.IConfiguratorIntegrationEvents` and add implementation to DI

```csharp

services.AddScoped<IConfiguratorIntegrationEvents, SampleConfiguratorIntegrationEvents>();

```

## Contribute

### Build Angular UI

```shell

cd /configurator-ui

npm install

npm run ng build

```

### Build backend

Build and run project `SampleSystem.WebApiCore`

