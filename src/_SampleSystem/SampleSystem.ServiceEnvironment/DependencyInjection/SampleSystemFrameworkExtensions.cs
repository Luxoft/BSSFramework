using System;

using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.CustomReports.Domain;
using Framework.CustomReports.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.SerializeMetadata;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;
using SampleSystem.WebApiCore.CustomReports;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemFrameworkExtensions
{
    public static IServiceCollection RegisterGeneralBssFramework(this IServiceCollection services, IConfiguration configuration)
    {
        return services.RegisterLegacyBLLContext()
                       .AddEnvironment(configuration)
                       .AddControllerEnvironment()
                       .RegisterHierarchicalObjectExpander()
                       .RegisterAdditonalAuthorizationBLL();
    }


    private static IServiceCollection RegisterLegacyBLLContext(this IServiceCollection services)
    {
        services.AddScoped<TargetSystemServiceFactory>();
        services.AddScopedFrom((TargetSystemServiceFactory factory) => factory.Create<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase>(TargetSystemHelper.AuthorizationName));
        services.AddScopedFrom((TargetSystemServiceFactory factory) => factory.Create<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase>(TargetSystemHelper.ConfigurationName));
        services.AddScopedFrom((TargetSystemServiceFactory factory) => factory.Create<ISampleSystemBLLContext, PersistentDomainObjectBase>(tss => tss.IsMain));

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

        services.AddScoped<IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>>, SampleSystemLocalDBEventMessageSender>();
        services.AddScoped<IOperationEventListener<PersistentDomainObjectBase>, SampleSystemEventsSubscriptionManager>();

        services.AddScoped<IMessageSender<IDomainOperationSerializeData<Framework.Authorization.Domain.PersistentDomainObjectBase>>, AuthorizationLocalDBEventMessageSender>();

        services.AddScoped<SampleSystemAribaLocalDBEventMessageSender>();
        services.AddScoped<IOperationEventListener<PersistentDomainObjectBase>, SampleSystemAribaEventsSubscriptionManager>();


        services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());

        services.AddSingleton<IDefaultMailSenderContainer>(new DefaultMailSenderContainer("SampleSystem_Sender@luxoft.com"));

        services.AddScopedFrom<IBLLSimpleQueryBase<IEmployee>, IEmployeeBLLFactory>(factory => factory.Create());

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

    private static IServiceCollection RegisterMainBLL(this IServiceCollection services)
    {
        return services

               .AddSingleton<SampleSystemValidatorCompileCache>()

               .AddScoped<ISampleSystemValidator>(sp =>
                                                          new SampleSystemValidator(sp.GetRequiredService<ISampleSystemBLLContext>(), sp.GetRequiredService<SampleSystemValidatorCompileCache>()))

               .AddSingleton(new SampleSystemMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<PersistentDomainObjectBase>.OData))
               .AddScoped<ISampleSystemSecurityService, SampleSystemSecurityService>()
               .AddScoped<ISampleSystemBLLFactoryContainer, SampleSystemBLLFactoryContainer>()
               .AddScoped<ISampleSystemBLLContextSettings>(_ => new SampleSystemBLLContextSettings { TypeResolver = new[] { new SampleSystemBLLContextSettings().TypeResolver, TypeSource.FromSample<BusinessUnitSimpleDTO>().ToDefaultTypeResolver() }.ToComposite() })
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


    private static IServiceCollection AddEnvironment(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        //For middleware
        services.AddScoped<IWebApiDBSessionModeResolver, WebApiDBSessionModeResolver>();
        services.AddScoped<IWebApiCurrentMethodResolver, WebApiCurrentMethodResolver>();

        // Notifications
        services.RegisterNotificationJob<ISampleSystemBLLContext>();
        services.RegisterNotificationSmtp(configuration);
        services.RegisterRewriteReceiversDependencies(configuration);

        // Others
        services.RegisterUserAuthenticationServices();

        services.AddSingleton<IDateTimeService>(DateTimeService.Default);

        services.AddSingleton(new SubscriptionMetadataStore(new SampleSystemSubscriptionsMetadataFinder()));

        return services;
    }

    private static IServiceCollection AddControllerEnvironment(this IServiceCollection services)
    {
        services.AddSingleton<IWebApiExceptionExpander, WebApiExceptionExpander>();

        services.AddSingleton<IReportParameterValueService<ISampleSystemBLLContext, PersistentDomainObjectBase, SampleSystemSecurityOperationCode>, ReportParameterValueService<ISampleSystemBLLContext, PersistentDomainObjectBase, SampleSystemSecurityOperationCode>>();
        services.AddSingleton<ISystemMetadataTypeBuilder>(new SystemMetadataTypeBuilder<PersistentDomainObjectBase>(DTORole.All, typeof(PersistentDomainObjectBase).Assembly));
        services.AddSingleton<SampleSystemCustomReportsServiceEnvironment>();
        services.AddSingletonFrom((SampleSystemCustomReportsServiceEnvironment env) => env.ReportService);
        services.AddSingleton<ISecurityOperationCodeProvider<SampleSystemSecurityOperationCode>, SecurityOperationCodeProvider>();

        services.AddSingleton<IDBSessionEvaluator, DBSessionEvaluator>();

        services.AddSingleton<IContextEvaluator<IAuthorizationBLLContext>, ContextEvaluator<IAuthorizationBLLContext>>();
        services.AddSingleton<IContextEvaluator<IConfigurationBLLContext>, ContextEvaluator<IConfigurationBLLContext>>();
        services.AddSingletonFrom<IContextEvaluator<Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContext>, IContextEvaluator<IConfigurationBLLContext>>();
        services.AddSingleton<IContextEvaluator<ISampleSystemBLLContext>, ContextEvaluator<ISampleSystemBLLContext>>();

        services.AddScoped<IApiControllerBaseEvaluator<EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>, ApiControllerBaseSingleCallEvaluator<EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>>();
        services.AddScoped<IApiControllerBaseEvaluator<EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>, ApiControllerBaseSingleCallEvaluator<EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>>();
        services.AddScoped<IApiControllerBaseEvaluator<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>, ApiControllerBaseSingleCallEvaluator<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>>();

        return services;
    }


    private static IServiceCollection RegisterHierarchicalObjectExpander(this IServiceCollection services)
    {
        return services.AddSingleton<IHierarchicalRealTypeResolver, ProjectionHierarchicalRealTypeResolver>()
                       .AddScoped<IHierarchicalObjectExpanderFactory<Guid>, HierarchicalObjectExpanderFactory<PersistentDomainObjectBase, Guid>>();
    }

    private static IServiceCollection RegisterAdditonalAuthorizationBLL(this IServiceCollection services)
    {
        return services.AddScopedFrom<ISecurityTypeResolverContainer, ISampleSystemBLLContext>()
                       .AddScoped<IAuthorizationExternalSource, AuthorizationExternalSource<ISampleSystemBLLContext, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, SampleSystemSecurityOperationCode>>();
    }

}
