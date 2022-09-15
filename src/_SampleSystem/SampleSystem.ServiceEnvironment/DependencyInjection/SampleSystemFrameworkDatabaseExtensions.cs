﻿using System;
using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.ServiceModel.IAD;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using nuSpec.Abstraction;
using nuSpec.NHibernate;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DAL.NHibernate;
using SampleSystem.ServiceEnvironment.Database;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemFrameworkDatabaseExtensions
{
    public static IServiceCollection AddGeneralDatabaseSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        return services.AddDatabaseSettings(connectionString)
                       .AddDatabaseVisitors()
                       .RegisterSpecificationEvaluator();
    }

    private static IServiceCollection RegisterSpecificationEvaluator(this IServiceCollection services)
    {
        return services.AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>();
    }

    private static IServiceCollection AddDatabaseSettings(this IServiceCollection services, string connectionString)
    {
        return services.AddDatabaseSettings(setupObj => setupObj.AddEventListener<DefaultDBSessionEventListener>()
                                                                .AddEventListener<SubscriptionDBSessionEventListener>()

                                                                .SetEnvironment<SampleSystemNHibSessionEnvironment>()

                                                                .AddMapping(AuthorizationMappingSettings.CreateDefaultAudit(string.Empty))
                                                                .AddMapping(ConfigurationMappingSettings.CreateDefaultAudit(string.Empty))
                                                                .AddMapping(new SampleSystemMappingSettings(new DatabaseName(string.Empty, "app"), connectionString)));
    }
    private static IServiceCollection AddDatabaseVisitors(this IServiceCollection services)
    {
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPersistentItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPeriodItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDefaultItem>();

        services.AddSingleton<IIdPropertyResolver, IdPropertyResolver>();

        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<PersistentDomainObjectBase, Guid>>();

        services.AddScoped<IExpressionVisitorContainerItem, ExpressionVisitorContainerODataItem<ISampleSystemBLLContext, PersistentDomainObjectBase, Guid>>();

        services.AddScoped<IExpressionVisitorContainer, ExpressionVisitorAggregator>();

        return services;
    }
}
