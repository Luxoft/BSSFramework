using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events.Legacy;
using Framework.Notification;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;
using SampleSystem.Subscriptions.Metadata.Employee.Update;

using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemFrameworkExtensions
{
    public static IServiceCollection RegisterLegacyGeneralBssFramework(this IServiceCollection services)
    {
        return services.RegisterListeners()
                       .RegisterSupportServices()

                       // Legacy
                       .RegisterLegacyGenericServices()
                       .RegisterContextEvaluators()

                       .RegisterMainBLLContext()
                       .RegisterConfigurationTargetSystems();
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
        services.RegisterListeners(
            s => s.Add<FixDomainObjectEventRevisionNumberDALListener>()
                  .Add<DependencyDetailEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>>());

        services.RegisterSubscriptionManagers(
            s => s.Add<ExampleSampleSystemEventsSubscriptionManager>()
                  .Add<ExampleSampleSystemAribaEventsSubscriptionManager>());

        return services;
    }

    private static IServiceCollection RegisterSupportServices(this IServiceCollection services)
    {
        services.AddSingleton<ISecurityRuleParser, SampleSystemSecurityRuleParser>();

        //Custom ariba sender
        services.AddScoped<SampleSystemCustomAribaLocalDBEventMessageSender>();

        //For dto mapping
        services.AddScoped<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService>();

        //For mapping domain objects to dto events
        services
            .AddScoped<IDomainEventDTOMapper<PersistentDomainObjectBase>, RuntimeDomainEventDTOMapper<PersistentDomainObjectBase,
                ISampleSystemDTOMappingService, SampleSystem.Generated.DTO.EventDTOBase>>();

        // For notification
        services.AddSingleton<IDefaultMailSenderContainer>(new DefaultMailSenderContainer("SampleSystem_Sender@luxoft.com"));
        services.AddScoped<IEmployeeSource, EmployeeSource<Employee>>();

        // For subscription
        services.AddSingleton(new SubscriptionMetadataFinderAssemblyInfo(typeof(EmployeeUpdateSubscription).Assembly));
        return services;
    }
}
