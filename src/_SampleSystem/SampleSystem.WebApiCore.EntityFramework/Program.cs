using Bss.Platform.Api.Documentation;
using Bss.Platform.Api.Middlewares;
using Bss.Platform.Logging;

using CommonFramework.DependencyInjection;

using Framework.DomainDriven.WebApiNetCore;
using Framework.DomainDriven.WebApiNetCore.JsonConverter;
using Framework.DomainDriven.WebApiNetCore.Swagger;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;

using SampleSystem.ServiceEnvironment;

using SecuritySystem.Configurator;

namespace SampleSystem.WebApiCore;

public static class Program
{
    public static async Task Main(string[] args)
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
               .RegisterGeneralDependencyInjection(builder.Configuration)
               .AddPlatformApiDocumentation(
                   builder.Environment,
                   "SampleSystem API",
                   x =>
                   {
                       x.AddClientSecurityRule("SampleSystemSecurityRule");
                       x.CustomSchemaIds(t => t.FullName);
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
               .AddJsonOptions(
                   x =>
                   {
                       x.JsonSerializerOptions.Converters.Add(new UtcDateTimeJsonConverter());
                       x.JsonSerializerOptions.Converters.Add(new PeriodJsonConverter());
                   });

        builder.Services
               .AddValidator(new DuplicateServiceUsageValidator([typeof(ILoggerFactory)]))
               .Validate();

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

        await app.RunAsync();
    }
}
