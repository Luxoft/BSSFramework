using System;
using System.Collections.Generic;

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
        services.AddScoped<TargetSystemServiceFactory>();
        services.AddScoped(sp => sp.GetRequiredService<TargetSystemServiceFactory>().Create<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase>(TargetSystemHelper.AuthorizationName));
        services.AddScoped(sp => sp.GetRequiredService<TargetSystemServiceFactory>().Create<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase>(TargetSystemHelper.ConfigurationName));
        services.AddScoped(sp => sp.GetRequiredService<TargetSystemServiceFactory>().Create<ISampleSystemBLLContext, SampleSystem.Domain.PersistentDomainObjectBase>(tss => tss.IsMain));

        services.AddSingleton<IInitializeManager, InitializeManager>();

        services.AddScoped<IBeforeTransactionCompletedDALListener, DenormalizeHierarchicalDALListener<ISampleSystemBLLContext, PersistentDomainObjectBase, NamedLock, NamedLockOperation>>();
        services.AddScoped<IBeforeTransactionCompletedDALListener, FixDomainObjectEventRevisionNumberDALListener>();
        services.AddScoped<IBeforeTransactionCompletedDALListener, PermissionWorkflowDALListener>();

        services.AddScoped<DefaultAuthDALListener>();

        services.AddScopedFrom<IBeforeTransactionCompletedDALListener, DefaultAuthDALListener>();
        services.AddScopedFrom<IManualEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>, DefaultAuthDALListener>();

        services.AddScoped<EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>();
        services.AddScoped<IAuthorizationDTOMappingService, AuthorizationServerPrimitiveDTOMappingService>();

        services.AddScoped<EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>();
        services.AddScoped<IConfigurationDTOMappingService, ConfigurationServerPrimitiveDTOMappingService>();

        services.AddScoped<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>();
        services.AddScoped<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService>();

        services.AddScoped<IOperationEventSenderContainer<PersistentDomainObjectBase>, OperationEventSenderContainer<PersistentDomainObjectBase>>();

        services.AddScoped<IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>>, SampleSystemLocalDBEventMessageSender>();
        services.AddScoped<IOperationEventListener<PersistentDomainObjectBase>, SampleSystemEventsSubscriptionManager>();

        services.AddScoped<IMessageSender<IDomainOperationSerializeData<Framework.Authorization.Domain.PersistentDomainObjectBase>>, AuthorizationLocalDBEventMessageSender>();

        services.AddScoped<SampleSystemAribaLocalDBEventMessageSender>();
        services.AddScoped<IOperationEventListener<PersistentDomainObjectBase>, SampleSystemAribaEventsSubscriptionManager>();


        services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());

        services.AddSingleton<IDefaultMailSenderContainer>(new DefaultMailSenderContainer("SampleSystem_Sender@luxoft.com"));

        services.AddScoped<IBLLSimpleQueryBase<Framework.Persistent.IEmployee>>(sp => sp.GetRequiredService<IEmployeeBLLFactory>().Create());

        services.RegisterHierarchicalObjectExpander();

        services.RegisterAdditonalAuthorizationBLL();


        services.RegisterGenericServices();
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
        return services.AddScopedFrom<ISecurityTypeResolverContainer, ISampleSystemBLLContext>()
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
                .AddScoped<ISampleSystemBLLContextSettings>(_ => new SampleSystemBLLContextSettings { TypeResolver  = new[] { new SampleSystemBLLContextSettings().TypeResolver, TypeSource.FromSample<BusinessUnitSimpleDTO>().ToDefaultTypeResolver() }.ToComposite() })
                .AddScopedFromLazyInterfaceImplement<ISampleSystemBLLContext, SampleSystemBLLContext>()

                .AddScopedFrom<ISecurityOperationResolver<PersistentDomainObjectBase, SampleSystemSecurityOperationCode>, ISampleSystemBLLContext>()
                .AddScopedFrom<IDisabledSecurityProviderContainer<PersistentDomainObjectBase>, ISampleSystemSecurityService>()
                .AddScopedFrom<ISampleSystemSecurityPathContainer, ISampleSystemSecurityService>()
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
