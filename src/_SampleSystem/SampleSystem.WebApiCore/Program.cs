using Bss.Platform.Api.Documentation;
using Bss.Platform.Api.Middlewares;
using Bss.Platform.Events;
using Bss.Platform.Logging;

using Framework.Configurator;
using Framework.Configurator.Interfaces;
using Framework.DependencyInjection;
using Framework.DomainDriven.WebApiNetCore;
using Framework.DomainDriven.WebApiNetCore.JsonConverter;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;

using SampleSystem.BLL._Command.CreateClassA.Integration;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;
using SampleSystem.WebApiCore.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
       .AddJsonFile("appsettings.json", false, true)
       .AddEnvironmentVariables($"{nameof(SampleSystem)}_");

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
       .AddPlatformApiDocumentation(builder.Environment, "SampleSystem API", x => x.CustomSchemaIds(t => t.FullName))
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

builder.Services.AddAuthorization(
    options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
    });

builder.Services.AddControllers(x => x.EnableEndpointRouting = false)
       .AddJsonOptions(x =>
                       {
                           x.JsonSerializerOptions.Converters.Add(new UtcDateTimeJsonConverter());
                           x.JsonSerializerOptions.Converters.Add(new PeriodJsonConverter());
                       });

if (builder.Environment.IsProduction())
{
    builder.Services.AddHangfireBss(builder.Configuration.GetConnectionString("DefaultConnection"));
}

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
    app.UseHangfireBss(builder.Configuration, JobList.RunAll);
}

app.Run();
