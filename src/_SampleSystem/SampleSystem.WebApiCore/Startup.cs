﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using DotNetCore.CAP;

using Framework.Core;
using Framework.DependencyInjection;
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

using SampleSystem.BLL;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.NewtonsoftJson;

namespace SampleSystem.WebApiCore
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
                    new OpenApiInfo { Title = "SampleSystem", Version = "v1" },
                    new List<string> { Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml") });

            //// services.AddAuthentication()
            ////         .AddCapAuth<ISampleSystemBLLContext>();

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

            services.AddWorkflowCore(this.Configuration);
            services.AddAuthWorkflow();
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
                .UseCorrelationId("SampleSystem_{0}")

                .UseHttpsRedirection()
                .UseRouting()
                //// .UseAuthentication()
                //// .UseAuthorization()
                .UseEndpoints(z => z.MapControllers());

            if (env.IsProduction())
            {
                app.UseMetricsAllMiddleware();
                this.UseHangfireBss(app);
            }

            app.UseCapDashboard();

            app.ApplicationServices.StartWorkflow();
        }

        private void UseHangfireBss(IApplicationBuilder app)
        {
            var environment = LazyHelper.Create(
                            () =>
                            {
                                var serviceProvider = new ServiceCollection()
                                                            .AddEnvironment(this.Configuration)
                                                            .BuildServiceProvider();

                                return serviceProvider.GetRequiredService<SampleSystemServiceEnvironment>();
                            });

            app.UseHangfireBss(
                this.Configuration,
                z =>
                {
                    JobList.RunAll(z);
                },
                authorizationFilter: new SampleSystemHangfireAuthorization(environment));
        }
    }
}
