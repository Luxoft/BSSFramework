using Bss.Platform.Api.Documentation;
using Bss.Platform.Api.Middlewares;
using Bss.Platform.Events;
using Bss.Platform.Logging;

using Framework.Configurator;
using Framework.Configurator.Interfaces;
using Framework.DependencyInjection;
using Framework.DomainDriven.Setup;
using Framework.DomainDriven.WebApiNetCore;
using Framework.DomainDriven.WebApiNetCore.JsonConverter;
using Framework.DomainDriven.WebApiNetCore.Swagger;
using Framework.HangfireCore;
using Framework.NotificationCore.Jobs;

using Hangfire;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SampleSystem.BLL._Command.CreateClassA.Integration;
using SampleSystem.ServiceEnvironment;
using SampleSystem.ServiceEnvironment.Jobs;
using SampleSystem.WebApiCore.Services;

namespace SampleSystem.WebApiCore;

public static class GenericProgram
{
    public static async Task Main(string[] args, IBssFrameworkExtension extension)
    {
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
               .RegisterGeneralDependencyInjection(builder.Configuration, s => s.AddExtensions(extension))
               .AddScoped<IConfiguratorIntegrationEvents, SampleConfiguratorIntegrationEvents>()
               .AddPlatformApiDocumentation(
                   builder.Environment,
                   "SampleSystem API",
                   x =>
                   {
                       x.AddClientSecurityRule("SampleSystemSecurityRule");
                       x.CustomSchemaIds(t => t.FullName);
                   })
               .AddPlatformIntegrationEvents<IntegrationEventProcessor>(
                   typeof(ClassACreatedEvent).Assembly,
                   x =>
                   {
                       x.SqlServer.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
                       x.MessageQueue.Enable = false;
                   })
               .AddConfigurator(setup => setup.AddEvents().AddApplicationVariables())
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
               .AddJsonOptions(
                   x =>
                   {
                       x.JsonSerializerOptions.Converters.Add(new UtcDateTimeJsonConverter());
                       x.JsonSerializerOptions.Converters.Add(new PeriodJsonConverter());
                   });

        builder.Services.AddHangfireBss(
            builder.Configuration,
            s => s.AddJob<SampleJob>(new JobSettings { DisplayName = "SampleDisplayName" })
                  .AddJob<ISendNotificationsJob>((job, ct) => job.ExecuteAsync(ct), new JobSettings { CronTiming = Cron.Never() }));

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

        app.UseHangfireBss();

        await app.RunAsync();
    }
}
