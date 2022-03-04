﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using DotNetCore.CAP;

using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.SecuritySystem;
using Framework.WebApi.Utils;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.ServiceEnvironment;
using WorkflowSampleSystem.WebApiCore.NewtonsoftJson;

namespace WorkflowSampleSystem.WebApiCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = env;
        }

        public IWebHostEnvironment HostingEnvironment { get; }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if(this.HostingEnvironment.IsProduction())
            {
                services
                    .AddMetricsBss(this.Configuration, 0.5);
            }

            services
                .RegisterDependencyInjections(this.Configuration)
                .AddApiVersion()
                .AddSwaggerBss(
                    new OpenApiInfo { Title = "WorkflowSampleSystem", Version = "v1" },
                    new List<string> { Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml") });

            services.AddMediatR(Assembly.GetAssembly(typeof(EmployeeBLL)));

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
                services.AddHangfireBss(this.Configuration);

            }

            services.RegisterLegacyBLLContext();
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

            app
                .UseDefaultExceptionsHandling()
                .UseCorrelationId("WorkflowSampleSystem_{0}")

                .UseHttpsRedirection()
                .UseRouting()
                .UseEndpoints(z => z.MapControllers());

            if (env.IsProduction())
            {
                app.UseMetricsAllMiddleware();
            }

            app.UseCapDashboard();

        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterLegacyBLLContext(this IServiceCollection services)
        {
            services.RegisterEvaluateScopeManager<IWorkflowSampleSystemBLLContext>();
            services.RegisterAuthorizationSystem();

            services.RegisterAuthorizationBLL();
            services.RegisterConfigurationBLL();
            services.RegisterWorkflowBLL();
            services.RegisterMainBLL();

            return services;
        }



        public static IServiceCollection RegisterMainBLL(this IServiceCollection services)
        {
            return services

                   .AddScopedTransientByContainer(c => c.MainContext)
                   .AddScopedTransientByContainer<ISecurityOperationResolver<PersistentDomainObjectBase, WorkflowSampleSystemSecurityOperationCode>>(c => c.MainContext)
                   .AddScopedTransientByContainer<IDisabledSecurityProviderContainer<PersistentDomainObjectBase>>(c => c.MainContext.SecurityService)
                   .AddScopedTransientByContainer<IWorkflowSampleSystemSecurityPathContainer>(c => c.MainContext.SecurityService)
                   .AddScopedTransientByContainer(c => c.MainContext.GetQueryableSource())
                   .AddScopedTransientByContainer(c => c.MainContext.SecurityExpressionBuilderFactory)

                   .AddScoped<IAccessDeniedExceptionService<PersistentDomainObjectBase>, AccessDeniedExceptionService<PersistentDomainObjectBase, Guid>>()
                   .Self(WorkflowSampleSystemSecurityServiceBase.Register)
                   .Self(WorkflowSampleSystemBLLFactoryContainer.RegisterBLLFactory);
        }

        public static IServiceCollection AddScopedTransientByContainer<T>(this IServiceCollection services, Func<IServiceEnvironmentBLLContextContainer<IWorkflowSampleSystemBLLContext>, T> func)
            where T : class
        {
            return services.AddScopedTransientFactory(sp => sp.GetRequiredService<IEvaluateScopeManager<IWorkflowSampleSystemBLLContext>>()
                                                              .Pipe(manager => FuncHelper.Create(() => func(manager.CurrentBLLContextContainer))));
        }

        public static IServiceCollection RegisterDependencyInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEnvironment(configuration);

            return services;
        }
    }
}
