﻿using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events;
using Framework.Events.DTOMapper;
using Framework.Notification;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;

using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemFrameworkExtensions
{
    public static IServiceCollection RegisterGeneralBssFramework(this IServiceCollection services)
    {
        return services.RegisterGenericServices()
                       .RegisterWebApiGenericServices()
                       .RegisterListeners()
                       .RegisterSupportServices()

                       // Legacy
                       .RegisterLegacyGenericServices()
                       .RegisterContextEvaluators()

                       .RegisterMainBLLContext()
                       .RegisterConfigurationTargetSystems()
                       .RegisterContextEvaluator();
    }

    private static IServiceCollection RegisterMainBLLContext(this IServiceCollection services)
    {
        return services
               .AddSingleton<SampleSystemValidationMap>()
               .AddSingleton<SampleSystemValidatorCompileCache>()

               .AddScoped<ISampleSystemValidator, SampleSystemValidator>()

               .AddSingleton(new SampleSystemMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<PersistentDomainObjectBase>.OData))
               .AddScoped<ISampleSystemBLLFactoryContainer, SampleSystemBLLFactoryContainer>()
               .AddScoped<ISampleSystemBLLContextSettings>(_ => new SampleSystemBLLContextSettings { TypeResolver = new[] { new SampleSystemBLLContextSettings().TypeResolver, TypeSource.FromSample<BusinessUnitSimpleDTO>().ToDefaultTypeResolver() }.ToComposite() })
               .AddScopedFromLazyInterfaceImplement<ISampleSystemBLLContext, SampleSystemBLLContext>()

                //.AddScoped<ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>, SampleSystemSecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>>()

               .Self(SampleSystemBLLFactoryContainer.RegisterBLLFactory);
    }

    private static IServiceCollection RegisterConfigurationTargetSystems(this IServiceCollection services)
    {
        services.AddScoped<TargetSystemServiceFactory>();

        services.AddScopedFrom((TargetSystemServiceFactory factory) => factory.Create<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase>(TargetSystemHelper.AuthorizationName));
        services.AddScopedFrom((TargetSystemServiceFactory factory) => factory.Create<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase>(TargetSystemHelper.ConfigurationName));
        services.AddScopedFrom((TargetSystemServiceFactory factory) => factory.Create<ISampleSystemBLLContext, PersistentDomainObjectBase>(tss => tss.IsMain));

        return services;
    }

    private static IServiceCollection RegisterListeners(this IServiceCollection services)
    {
        services.AddScoped<FaultDALListener>();
        services.AddScopedFrom<IBeforeTransactionCompletedDALListener, FaultDALListener>();

        services.AddScoped<IBeforeTransactionCompletedDALListener, DenormalizeHierarchicalDALListener>();
        services.AddScoped<IBeforeTransactionCompletedDALListener, FixDomainObjectEventRevisionNumberDALListener>();
        services.AddScoped<IBeforeTransactionCompletedDALListener, PermissionWorkflowDALListener>();

        services.AddScoped<IEventOperationReceiver, DependencyDetailEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>>();

        services.AddScoped<IBeforeTransactionCompletedDALListener, DependencyDetailEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>>();

        services.AddKeyedScoped<IEventOperationReceiver, SampleSystemEventsSubscriptionManager>("BLL");
        services.AddScoped<IEventOperationReceiver, SampleSystemEventsSubscriptionManager>();

        services.AddKeyedScoped<IEventOperationReceiver, SampleSystemAribaEventsSubscriptionManager>("BLL");
        services.AddScoped<IEventOperationReceiver, SampleSystemAribaEventsSubscriptionManager>();

        return services;
    }

    private static IServiceCollection RegisterContextEvaluator(this IServiceCollection services)
    {
        services.AddScoped<IApiControllerBaseEvaluator<ISampleSystemBLLContext, ISampleSystemDTOMappingService>, ApiControllerBaseSingleCallEvaluator<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>();

        return services;
    }

    private static IServiceCollection RegisterSupportServices(this IServiceCollection services)
    {
        //For dto mapping
        services.AddScoped<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService>();

        //For mapping domain objects to dto events
        services
            .AddScoped<IDomainEventDTOMapper<PersistentDomainObjectBase>, RuntimeDomainEventDTOMapper<PersistentDomainObjectBase,
                ISampleSystemDTOMappingService, SampleSystem.Generated.DTO.EventDTOBase>>();

        // For NamedLocks
        services.AddSingleton(new NamedLockTypeInfo(typeof(SampleSystemNamedLock)));

        // For notification
        services.AddSingleton<IDefaultMailSenderContainer>(new DefaultMailSenderContainer("SampleSystem_Sender@luxoft.com"));
        services.AddScoped<IEmployeeSource, EmployeeSource<Employee>>();

        // For subscription
        services.AddSingleton(new SubscriptionMetadataStore(new SampleSystemSubscriptionsMetadataFinder()));

        // For expand tree
        services.RegisterHierarchicalObjectExpander();

        // Serilog
        services.AddSingleton(Serilog.Log.Logger);

        return services;
    }
}
