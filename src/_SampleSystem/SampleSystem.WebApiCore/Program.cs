using System.Reflection;

using Framework.Configurator;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.WebApiNetCore;
using Framework.SecuritySystem;
using Framework.WebApi.Utils;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;
using SampleSystem.WebApiCore.NewtonsoftJson;
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
       .UseSerilogBss(new Dictionary<string, string> { { "Version", Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString()! } });

if (builder.Environment.IsProduction())
    builder.Services.AddMetricsBss(builder.Configuration, 0.5);

builder.Services
       .RegisterGeneralDependencyInjection(builder.Configuration)
       .AddApiVersion()
       .AddSwaggerBss(
           new OpenApiInfo { Title = "SampleSystem", Version = "v1" },
           new List<string> { Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml") })
       .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
       .AddNegotiate();

builder.Services
       .AddConfigurator()
       .AddScoped<IConfiguratorIntegrationEvents, SampleConfiguratorIntegrationEvents>()
       .AddMvcBss()
       .AddNewtonsoftJson(
           x =>
           {
               x.SerializerSettings.Converters.Add(new UtcDateTimeJsonConverter());
               x.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
           });

if (builder.Environment.IsProduction())
{
    AppMetricsServiceCollectionExtensions.AddMetrics(builder.Services);
    builder.Services.AddHangfireBss(builder.Configuration.GetConnectionString("DefaultConnection"));
}

builder.Services.ValidateDuplicateDeclaration(typeof(ILoggerFactory));

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerBss(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection()
   .UseRouting()
   .UseDefaultExceptionsHandling()
   .UseCorrelationId("SampleSystem_{0}")
   .UseTryProcessDbSession()
   .UseWebApiExceptionExpander()
   .UseAuthentication()
   .UseAuthorization()
   .UseConfigurator()
   .UseEndpoints(z => z.MapControllers());

if (builder.Environment.IsProduction())
{
    app.UseMetricsAllMiddleware();

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
