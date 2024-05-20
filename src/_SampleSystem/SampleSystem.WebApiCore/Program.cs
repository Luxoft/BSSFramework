using Bss.Platform.Api.Documentation;
using Bss.Platform.Api.Middlewares;
using Bss.Platform.Events;
using Bss.Platform.Logging;

using Framework.Configurator;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.WebApiNetCore;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Http.Json;

using SampleSystem.BLL._Command.CreateClassA.Intergation;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;
using SampleSystem.WebApiCore.Extensions;
using SampleSystem.WebApiCore.Json;
using SampleSystem.WebApiCore.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
       .AddJsonFile("appsettings.json", false, true)
       .AddEnvironmentVariables();

builder.Host
       .UseDefaultServiceProvider(
           x =>
           {
               x.ValidateScopes = true;
               x.ValidateOnBuild = true;
           })
       .AddPlatformLogging();

builder.Services
       .RegisterGeneralDependencyInjection(builder.Configuration)
       .AddScoped<IConfiguratorIntegrationEvents, SampleConfiguratorIntegrationEvents>()
       .Configure<JsonOptions>(x => x.SerializerOptions.Converters.Add(new UtcDateTimeJsonConverter()))
       .AddSwaggerOld(builder.Environment)
       .AddPlatformIntegrationEvents<IntegrationEventProcessor>(
           typeof(ClassACreatedEvent).Assembly,
           x =>
           {
               x.SqlServer.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
               x.MessageQueue.Enable = false;
           })
       .AddConfigurator()
       .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
       .AddNegotiate();

builder.Services
       .AddMvc(x => x.EnableEndpointRouting = false);

if (builder.Environment.IsProduction()) builder.Services.AddHangfireBss(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.ValidateDuplicateDeclaration(typeof(ILoggerFactory));

var app = builder.Build();

app
    .UsePlatformErrorsMiddleware()
    .UseHttpsRedirection()
    .UseHsts()
    .UseRouting()
    .UseTryProcessDbSession()
    .UseWebApiExceptionExpander()
    .UseAuthentication()
    .UseAuthorization()
    .UseConfigurator()
    .UsePlatformApiDocumentation(builder.Environment)
    .UseEndpoints(x => x.MapControllers());

if (builder.Environment.IsProduction())
{
    var contextEvaluator = LazyInterfaceImplementHelper.CreateProxy(
        () =>
        {
            var serviceProvider = new ServiceCollection()
                                  .RegisterGeneralDependencyInjection(builder.Configuration)
                                  .ValidateDuplicateDeclaration()
                                  .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            return serviceProvider.GetRequiredService<IServiceEvaluator<IAuthorizationSystem>>();
        });

    app.UseHangfireBss(
        builder.Configuration,
        JobList.RunAll,
        authorizationFilter: new SampleSystemHangfireAuthorization(contextEvaluator));
}

app.Run();
