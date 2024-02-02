using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;

using IConfigurationBLLContext = Framework.Configuration.BLL.IConfigurationBLLContext;

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
        services.AddSingleton<IInitializeManager, InitializeManager>();

        services.AddScoped<IBeforeTransactionCompletedDALListener, DenormalizeHierarchicalDALListener<PersistentDomainObjectBase, NamedLock, NamedLockOperation>>();
        services.AddScoped<IBeforeTransactionCompletedDALListener, FixDomainObjectEventRevisionNumberDALListener>();
        services.AddScoped<IBeforeTransactionCompletedDALListener, PermissionWorkflowDALListener>();

        services.AddScoped<FaultDALListener>();
        services.AddScopedFrom<IBeforeTransactionCompletedDALListener, FaultDALListener>();

        services.AddScoped<DefaultAuthDALListener>();

        services.AddScopedFrom<IBeforeTransactionCompletedDALListener, DefaultAuthDALListener>();
        services.AddScopedFrom<IManualEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>, DefaultAuthDALListener>();

        services.AddScoped<EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>();
        services.AddScoped<IAuthorizationDTOMappingService, AuthorizationServerPrimitiveDTOMappingService>();

        services.AddScoped<EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>();
        services.AddScoped<IConfigurationDTOMappingService, ConfigurationServerPrimitiveDTOMappingService>();

        services.AddScoped<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>();
        services.AddScoped<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService>();

        services.AddScoped<IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>>, SampleSystemLocalDBEventMessageSender>();
        services.AddScoped<IOperationEventListener<PersistentDomainObjectBase>, SampleSystemEventsSubscriptionManager>();

        services.AddScoped<IMessageSender<IDomainOperationSerializeData<Framework.Authorization.Domain.PersistentDomainObjectBase>>, AuthorizationLocalDBEventMessageSender>();

        services.AddScoped<SampleSystemAribaLocalDBEventMessageSender>();
        services.AddScoped<IOperationEventListener<PersistentDomainObjectBase>, SampleSystemAribaEventsSubscriptionManager>();

        return services;
    }

    private static IServiceCollection RegisterContextEvaluator(this IServiceCollection services)
    {
        services.AddScoped<IApiControllerBaseEvaluator<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>, ApiControllerBaseSingleCallEvaluator<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>>();

        return services;
    }

    private static IServiceCollection RegisterSupportServices(this IServiceCollection services)
    {
        // For notification
        services.AddSingleton<IDefaultMailSenderContainer>(new DefaultMailSenderContainer("SampleSystem_Sender@luxoft.com"));
        services.AddScopedFrom<IEmployeeSource, EmployeeSource<Employee>>();

        // For subscription
        services.AddSingleton(new SubscriptionMetadataStore(new SampleSystemSubscriptionsMetadataFinder()));

        // For expand tree
        services.RegisterHierarchicalObjectExpander<PersistentDomainObjectBase>();

        // Serilog
        services.AddSingleton(Serilog.Log.Logger);

        return services;
    }
}
