﻿using System;

using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Events;
using Framework.HierarchicalExpand;
using Framework.QueryableSource;
using Framework.Security.Cryptography;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.BLL.Core.Jobs;
using SampleSystem.BLL.Jobs;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.WebApiCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLegacyBLLContext(this IServiceCollection services)
    {
        services.AddSingleton<IInitializeManager, InitializeManager>();

        services.AddScoped<IBeforeTransactionCompletedDALListener, DenormalizeHierarchicalDALListener<ISampleSystemBLLContext, PersistentDomainObjectBase, NamedLock, NamedLockOperation>>();
        services.AddScoped<IBeforeTransactionCompletedDALListener, FixDomainObjectEventRevisionNumberDALListener>();

        services.AddScoped<DefaultAuthDALListener>();

        services.AddScoped<IBeforeTransactionCompletedDALListener>(sp => sp.GetRequiredService<DefaultAuthDALListener>());
        services.AddScoped<IManualEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>>(sp => sp.GetRequiredService<DefaultAuthDALListener>());


        services.AddScoped<EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>();
        services.AddScoped<IAuthorizationDTOMappingService, AuthorizationServerPrimitiveDTOMappingService>();

        services.AddScoped<EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>();
        services.AddScoped<IConfigurationDTOMappingService, ConfigurationServerPrimitiveDTOMappingService>();

        services.AddScoped<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>();
        services.AddScoped<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService>();


        services.AddScoped<IOperationEventSenderContainer<PersistentDomainObjectBase>, OperationEventSenderContainer<PersistentDomainObjectBase>>();

        services.AddScoped<IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>>, SampleSystemLocalDBEventMessageSender>();
        services.AddScoped<IOperationEventListener<PersistentDomainObjectBase>, SampleSystemEventsSubscriptionManager>();

        services.AddScoped<IOperationEventListener<Framework.Authorization.Domain.PersistentDomainObjectBase>, AuthorizationEventsSubscriptionManager>();
        services.AddScoped<IMessageSender<IDomainOperationSerializeData<Framework.Authorization.Domain.PersistentDomainObjectBase>>, AuthorizationLocalDBEventMessageSender>();


        services.AddScoped<SampleSystemAribaLocalDBEventMessageSender>();
        services.AddScoped<IOperationEventListener<PersistentDomainObjectBase>, SampleSystemAribaEventsSubscriptionManager>();

        services.AddScoped<IStandardSubscriptionService, LocalDBSubscriptionService>();


        services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());

        services.AddSingleton<IDefaultMailSenderContainer>(new DefaultMailSenderContainer("SampleSystem_Sender@luxoft.com"));

        services.AddScoped<IBLLSimpleQueryBase<Framework.Persistent.IEmployee>>(sp => sp.GetRequiredService<IEmployeeBLLFactory>().Create());

        services.RegisterHierarchicalObjectExpander();

        services.RegisterAdditonalAuthorizationBLL();


        services.RegisterGenericBLLServices();
        services.RegisterAuthorizationSystem();

        services.RegisterAuthorizationBLL();
        services.RegisterConfigurationBLL();
        services.RegisterMainBLL();

        services.AddScoped<IAuthorizationValidator, SampleSystemCustomAuthValidator>();

        return services;
    }

    public static IServiceCollection RegisterHierarchicalObjectExpander(this IServiceCollection services)
    {
        return services.AddSingleton<IHierarchicalRealTypeResolver, ProjectionHierarchicalRealTypeResolver>()
                       .AddScoped<IHierarchicalObjectExpanderFactory<Guid>, HierarchicalObjectExpanderFactory<PersistentDomainObjectBase, Guid>>();
    }

    public static IServiceCollection RegisterAdditonalAuthorizationBLL(this IServiceCollection services)
    {
        return services.AddScoped<ISecurityTypeResolverContainer>(sp => sp.GetRequiredService<ISampleSystemBLLContext>())
                       .AddScoped<IAuthorizationExternalSource, AuthorizationExternalSource<ISampleSystemBLLContext, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, SampleSystemSecurityOperationCode>>();
    }

    public static IServiceCollection RegisterMainBLL(this IServiceCollection services)
    {
        return services

                .AddScoped(sp => sp.GetRequiredService<IDBSession>().GetDALFactory<PersistentDomainObjectBase, Guid>())

                .AddScoped<BLLSourceEventListenerContainer<PersistentDomainObjectBase>>()

                .AddSingleton<SampleSystemValidatorCompileCache>()

                .AddScoped<ISampleSystemValidator>(sp =>
                     new SampleSystemValidator(sp.GetRequiredService<ISampleSystemBLLContext>(), sp.GetRequiredService<SampleSystemValidatorCompileCache>()))

                .AddSingleton(new SampleSystemMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<PersistentDomainObjectBase>.OData))
                .AddScoped<ISampleSystemSecurityService, SampleSystemSecurityService>()
                .AddScoped<ISampleSystemBLLFactoryContainer, SampleSystemBLLFactoryContainer>()
                 .AddSingleton<ICryptService<CryptSystem>, CryptService<CryptSystem>>()
                .AddScoped<ISampleSystemBLLContextSettings, SampleSystemBLLContextSettings>()
                .AddLazyInterfaceImplementScoped<ISampleSystemBLLContext, SampleSystemBLLContext>()

                .AddScoped<ISecurityOperationResolver<PersistentDomainObjectBase, SampleSystemSecurityOperationCode>>(sp => sp.GetRequiredService<ISampleSystemBLLContext>())
                .AddScoped<IDisabledSecurityProviderContainer<PersistentDomainObjectBase>>(sp => sp.GetRequiredService<ISampleSystemSecurityService>())
                .AddScoped<ISampleSystemSecurityPathContainer>(sp => sp.GetRequiredService<ISampleSystemSecurityService>())
                .AddScoped<IQueryableSource<PersistentDomainObjectBase>, BLLQueryableSource<ISampleSystemBLLContext, PersistentDomainObjectBase, DomainObjectBase, Guid>>()
                .AddScoped<ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>>()
                .AddScoped<IAccessDeniedExceptionService<PersistentDomainObjectBase>, AccessDeniedExceptionService<PersistentDomainObjectBase, Guid>>()

                .Self(SampleSystemSecurityServiceBase.Register)
                .Self(SampleSystemBLLFactoryContainer.RegisterBLLFactory);
    }

    public static IServiceCollection RegisterDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEnvironment(configuration);

        services.AddScoped<ISampleJob, SampleJob>();

        return services;
    }
}
