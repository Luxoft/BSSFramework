using System.Reflection;

using Framework.Authorization.ApproveWorkflow;
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
using SampleSystem.WebApiCore.NewtonsoftJson;
using SampleSystem.WebApiCore.Services;

namespace SampleSystem.WebApiCore;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        this.Configuration = configuration;
        this.HostingEnvironment = env;
    }

    private IWebHostEnvironment HostingEnvironment { get; }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        if (this.HostingEnvironment.IsProduction())
        {
            services.AddMetricsBss(this.Configuration, 0.5);
        }

        services.RegisterGeneralDependencyInjection(this.Configuration)
                .AddApiVersion()
                .AddSwaggerBss(
                    new OpenApiInfo { Title = "SampleSystem", Version = "v1" },
                    new List<string>
                    {
                        Path.Combine(
                            AppContext.BaseDirectory,
                            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml")
                    });

        services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();

        services.AddConfigurator();
        services.AddScoped<IConfiguratorIntegrationEvents, SampleConfiguratorIntegrationEvents>();

        services
            .AddMvcBss()
            .AddNewtonsoftJson(
                z =>
                {
                    z.SerializerSettings.Converters.Add(new UtcDateTimeJsonConverter());
                    z.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                });

        if (this.HostingEnvironment.IsProduction())
        {
            services.AddMetrics();
            services.AddHangfireBss(this.Configuration.GetConnectionString("DefaultConnection"));
        }

        services.ValidateDuplicateDeclaration(typeof(ILoggerFactory));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionProvider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwaggerBss(versionProvider);
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

        if (env.IsProduction())
        {
            app.UseMetricsAllMiddleware();
            this.UseHangfireBss(app);
        }

        app.ApplicationServices.RegisterAuthWorkflow();
        app.ApplicationServices.GetRequiredService<WorkflowManager>().Start();
    }

    private void UseHangfireBss(IApplicationBuilder app)
    {
        var contextEvaluator = LazyInterfaceImplementHelper.CreateProxy(
            () =>
            {
                var serviceProvider = new ServiceCollection()
                                      .RegisterGeneralDependencyInjection(
                                          this.Configuration)
                                      .ValidateDuplicateDeclaration()
                                      .BuildServiceProvider(
                                          new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

                return serviceProvider.GetRequiredService<IServiceEvaluator<IAuthorizationSystem>>();
            });

        app.UseHangfireBss(
            this.Configuration,
            JobList.RunAll,
            authorizationFilter: new SampleSystemHangfireAuthorization(contextEvaluator));
    }
}
