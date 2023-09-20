using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.SerializeMetadata;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemFrameworkExtensions
{
    public static IServiceCollection RegisterGeneralBssFramework(this IServiceCollection services)
    {
        return services.RegisterGenericServices()
                       .RegisterDomainServices()
                       .RegisterWebApiGenericServices()
                       .RegisterListeners()
                       .RegisterSupportServices()

                       // Legacy

                       .RegisterLegacyGenericServices()
                       .RegisterLegacyHierarchicalObjectExpander()
                       .RegisterContextEvaluators()

                       .RegisterMainBLLContext()
                       .RegisterConfigurationTargetSystems()
                       .RegisterContextEvaluator()

                       .RegisterCustomReports();
    }

    private static IServiceCollection RegisterMainBLLContext(this IServiceCollection services)
    {
        return services
               .AddSingleton<SampleSystemValidationMap>()
               .AddSingleton<SampleSystemValidatorCompileCache>()

               .AddScoped<ISampleSystemValidator, SampleSystemValidator>()

               .AddSingleton(new SampleSystemMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<PersistentDomainObjectBase>.OData))
               .AddScoped<IRootSecurityService<PersistentDomainObjectBase>, RootSecurityService<ISampleSystemBLLContext, PersistentDomainObjectBase>>()
               .AddScoped<ISampleSystemBLLFactoryContainer, SampleSystemBLLFactoryContainer>()
               .AddScoped<ISampleSystemBLLContextSettings>(_ => new SampleSystemBLLContextSettings { TypeResolver = new[] { new SampleSystemBLLContextSettings().TypeResolver, TypeSource.FromSample<BusinessUnitSimpleDTO>().ToDefaultTypeResolver() }.ToComposite() })
               .AddScopedFromLazyInterfaceImplement<ISampleSystemBLLContext, SampleSystemBLLContext>()

               .AddScoped<ITrackingService<PersistentDomainObjectBase>, TrackingService<PersistentDomainObjectBase>>()

               .AddScoped<IQueryableSource<PersistentDomainObjectBase>, BLLQueryableSource<ISampleSystemBLLContext, PersistentDomainObjectBase, Guid>>()
               .AddScoped<ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>>()
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
        services.AddSingleton<IContextEvaluator<ISampleSystemBLLContext>, ContextEvaluator<ISampleSystemBLLContext>>();
        services.AddScoped<IApiControllerBaseEvaluator<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>, ApiControllerBaseSingleCallEvaluator<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>>();

        return services;
    }

    private static IServiceCollection RegisterCustomReports(this IServiceCollection services)
    {
        services.AddSingleton<ISystemMetadataTypeBuilder>(new SystemMetadataTypeBuilder<PersistentDomainObjectBase>(DTORole.All, typeof(PersistentDomainObjectBase).Assembly));

        return services;
    }

    private static IServiceCollection RegisterSupportServices(this IServiceCollection services)
    {
        // For auth
        services.AddScopedFrom<ISecurityTypeResolverContainer, ISampleSystemBLLContext>();
        services.AddScoped<IAuthorizationExternalSource, AuthorizationExternalSource<ISampleSystemBLLContext, PersistentDomainObjectBase, AuditPersistentDomainObjectBase>>();

        // For notification
        services.AddSingleton<IDefaultMailSenderContainer>(new DefaultMailSenderContainer("SampleSystem_Sender@luxoft.com"));
        services.AddScopedFrom<IBLLSimpleQueryBase<IEmployee>, IEmployeeBLLFactory>(factory => factory.Create());

        // For subscription
        services.AddSingleton(new SubscriptionMetadataStore(new SampleSystemSubscriptionsMetadataFinder()));

        // For expand tree
        services.RegisterHierarchicalObjectExpander<PersistentDomainObjectBase>();

        return services;
    }

    private static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        return services.RegisterAuthorizationSystemDomainServices(

            rb =>

                rb.Add<Employee>(
                      db =>
                          db.SetView(SampleSystemSecurityOperation.EmployeeView)
                            .SetEdit(SampleSystemSecurityOperation.EmployeeEdit)
                            .SetCustomService<SampleSystemEmployeeSecurityService>())

                  .Add<BusinessUnit>(
                      db => db.SetView(SampleSystemSecurityOperation.BusinessUnitView)
                              .SetEdit(SampleSystemSecurityOperation.BusinessUnitEdit)
                              .SetPath(SecurityPath<BusinessUnit>.Create(fbu => fbu)))

                  .Add<BusinessUnitType>(
                      db => db.SetView(SampleSystemSecurityOperation.BusinessUnitTypeView)
                              .SetEdit(SampleSystemSecurityOperation.BusinessUnitTypeEdit))

                  .Add<BusinessUnitManagerCommissionLink>(
                      db => db.SetView(SampleSystemSecurityOperation.BusinessUnitManagerCommissionLinkView)
                              .SetEdit(SampleSystemSecurityOperation.BusinessUnitManagerCommissionLinkEdit)
                              .SetPath(SecurityPath<BusinessUnitManagerCommissionLink>.Create(v => v.BusinessUnit)))

                  .Add<BusinessUnitHrDepartment>(
                      db => db.SetView(SampleSystemSecurityOperation.BusinessUnitHrDepartmentView)
                              .SetEdit(SampleSystemSecurityOperation.BusinessUnitHrDepartmentEdit)
                              .SetPath(SecurityPath<BusinessUnitHrDepartment>.Create(v => v.BusinessUnit).And(v => v.HRDepartment.Location)))

                  .Add<ManagementUnit>(
                      db => db.SetView(SampleSystemSecurityOperation.ManagementUnitView)
                              .SetEdit(SampleSystemSecurityOperation.ManagementUnitEdit)
                              .SetPath(SecurityPath<ManagementUnit>.Create(mbu => mbu)))

                  .Add<ManagementUnitAndBusinessUnitLink>(
                      db => db.SetView(SampleSystemSecurityOperation.ManagementUnitAndBusinessUnitLinkView)
                              .SetEdit(SampleSystemSecurityOperation.ManagementUnitAndBusinessUnitLinkEdit)
                              .SetPath(SecurityPath<ManagementUnitAndBusinessUnitLink>.Create(v => v.BusinessUnit).And(v => v.ManagementUnit)))

                  .Add<ManagementUnitAndHRDepartmentLink>(
                      db => db.SetView(SampleSystemSecurityOperation.ManagementUnitAndHRDepartmentLinkView)
                              .SetEdit(SampleSystemSecurityOperation.ManagementUnitAndHRDepartmentLinkEdit)
                              .SetPath(SecurityPath<ManagementUnitAndHRDepartmentLink>.Create(v => v.ManagementUnit).And(v => v.HRDepartment.Location)))

                  .Add<EmployeeSpecialization>(
                      db => db.SetView(SampleSystemSecurityOperation.EmployeeSpecializationView))

                  .Add<EmployeeRole>(
                      db => db.SetView(SampleSystemSecurityOperation.EmployeeRoleView))

                  .Add<EmployeeRoleDegree>(
                      db => db.SetView(SampleSystemSecurityOperation.EmployeeRoleDegreeView))

                  .Add<HRDepartment>(
                      db => db.SetView(SampleSystemSecurityOperation.HRDepartmentView)
                              .SetEdit(SampleSystemSecurityOperation.HRDepartmentEdit))

                  .Add<Location>(
                      db => db.SetView(SampleSystemSecurityOperation.LocationView)
                              .SetEdit(SampleSystemSecurityOperation.LocationEdit))

                  .Add<Country>(
                      db => db.SetView(SampleSystemSecurityOperation.CountryView)
                              .SetEdit(SampleSystemSecurityOperation.CountryEdit))

                  .Add<CompanyLegalEntity>(
                      db => db.SetView(SampleSystemSecurityOperation.CompanyLegalEntityView)
                              .SetEdit(SampleSystemSecurityOperation.CompanyLegalEntityEdit))

                  .Add<EmployeePosition>(
                      db => db.SetView(SampleSystemSecurityOperation.EmployeePositionView)
                              .SetEdit(SampleSystemSecurityOperation.EmployeePositionEdit)
                              .SetPath(SecurityPath<EmployeePosition>.Create(position => position.Location)))

                  .Add<EmployeePersonalCellPhone>(
                      db => db.SetView(SampleSystemSecurityOperation.EmployeePersonalCellPhoneView)
                              .SetEdit(SampleSystemSecurityOperation.EmployeePersonalCellPhoneEdit))

                  .Add<TestRootSecurityObj>(
                      db => db.SetView(SampleSystemSecurityOperation.EmployeeView)
                              .SetPath(SecurityPath<TestRootSecurityObj>.Create(v => v.BusinessUnit).And(v => v.Location)))

                  .Add<TestPerformanceObject>(
                      db => db.SetView(SampleSystemSecurityOperation.EmployeeView)
                              .SetPath(SecurityPath<TestPerformanceObject>.Create(v => v.Location, SingleSecurityMode.Strictly)
                                                                          .And(v => v.Employee, SingleSecurityMode.Strictly)
                                                                          .And(v => v.BusinessUnit, SingleSecurityMode.Strictly)
                                                                          .And(v => v.ManagementUnit, SingleSecurityMode.Strictly)))

                  .Add<TestPlainAuthObject>(
                      db => db.SetView(SampleSystemSecurityOperation.EmployeeView)
                              .SetPath(SecurityPath<TestPlainAuthObject>.Create(v => v.Location)
                                                                        .And(v => v.Items.Select(item => item.BusinessUnit), ManySecurityPathMode.All)
                                                                        .And(v => v.Items.Select(item => item.ManagementUnit), ManySecurityPathMode.All)))

                  .Add<AuthPerformanceObject>(
                      db => db.SetView(SampleSystemSecurityOperation.BusinessUnitView)
                              .SetPath(SecurityPath<AuthPerformanceObject>.Create(v => v.BusinessUnit)
                                                                          .And(v => v.ManagementUnit)
                                                                          .And(v => v.Location)
                                                                          .And(v => v.Employee)))

                  .Add<EmployeePhoto>(
                      db => db.SetView(SampleSystemSecurityOperation.EmployeeView)
                              .SetPath(SecurityPath<EmployeePhoto>.Create(employeePhoto => employeePhoto.Employee.CoreBusinessUnit)))

                  .Add<ManagementUnitFluentMapping>(
                      db => db.SetView(SampleSystemSecurityOperation.ManagementUnitView)
                              .SetEdit(SampleSystemSecurityOperation.ManagementUnitEdit))

                  .Add<Example1>(
                      db => db.SetView(SampleSystemSecurityOperation.LocationView)
                              .SetEdit(SampleSystemSecurityOperation.LocationEdit))
            );
    }
}
