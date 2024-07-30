using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.Setup;
using Framework.Events.Legacy;
using Framework.Notification;
using Framework.WebApi.Utils.SL;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Domain.IntegrationVersions;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;
using SampleSystem.Subscriptions.Metadata.Employee.Update;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemFrameworkExtensions
{
    public static IBssFrameworkSettings AddListeners(this IBssFrameworkSettings settings)
    {
        return settings.AddListener<SubscriptionDALListener>()
                       .AddListener<ExampleFaultDALListener>(true)
                       .AddListener<FixDomainObjectEventRevisionNumberDALListener>()
                       .AddListener<DependencyDetailEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>>();
    }

    public static IBssFrameworkSettings AddSubscriptionManagers(this IBssFrameworkSettings settings)
    {
        return settings.AddSubscriptionManager<ExampleSampleSystemEventsSubscriptionManager>()
                       .AddSubscriptionManager<ExampleSampleSystemAribaEventsSubscriptionManager>();
    }

    public static IBssFrameworkSettings AddBLLSystem(this IBssFrameworkSettings settings)
    {
        return settings.AddBLLSystem<ISampleSystemBLLContext, SampleSystemBLLContext>();
    }

    public static IBssFrameworkSettings AddConfigurationTargetSystems(this IBssFrameworkSettings settings) =>
        settings.AddConfigurationTargetSystems(
            tsSettings =>
                tsSettings.AddTargetSystem<ISampleSystemBLLContext, PersistentDomainObjectBase>(
                    nameof(SampleSystem),
                    new Guid("{2D362091-7DAC-4BEC-A5AB-351B93B338D7}"),
                    true,
                    true,
                    new DomainTypeInfo[]
                        {
                            new(typeof(AnotherSqlParserTestObj), new Guid("{2328B905-DD6F-4304-A406-09A8D56A365F}")),
                            new(typeof(BusinessUnit), new Guid("5C326B10-B4B4-402C-BCCE-A311016CB715")),
                            new(typeof(Country), new Guid("{C6030B2D-16F1-4854-9FAB-8A69B7FFAC6C}")),
                            new(typeof(Location), new Guid("CACA9DB4-9DA6-48AA-9FD3-A311016CB715")),
                            new(typeof(Employee), new Guid("{AA46DA53-9B21-4DEC-9C70-720BDA1CB198}")),
                            new(typeof(EmployeeCellPhone), new Guid("9D3EF98C-B857-40EF-A170-DB1285E4CE28")),
                            new(typeof(TestRestrictionObject), new Guid("{1A7C78AD-9371-4DAF-895C-EF6E5A8A0350}")),
                            new(typeof(HRDepartment), new Guid("0BE31997-C4CD-449E-9394-A311016CB715")),
                            new(typeof(IntegrationVersionContainer1), new Guid("D1972415-C65B-42D7-ADBB-561B03935E70")),
                            new(typeof(Principal), new Guid("DB66670A-6A1A-4F0E-BDAE-20ED291B2ACC")),
                            new(typeof(Project), new Guid("{D79C7F5B-2968-4ACA-91FA-E12B36F121E2}")),
                            new(typeof(SqlParserTestObj), new Guid("{4963D86E-5650-41E0-BDBA-0274FF2CF375}")),
                            new(typeof(SqlParserTestObjContainer), new Guid("{6502514C-2B88-40BF-8D01-C3DFAB008599}")),
                            new(typeof(ManagementUnitFluentMapping), new Guid("11E78AEF-9512-46E0-A33D-AAE58DC7E18C")),
                            new(typeof(IntegrationVersionContainer2), new Guid("4FAB2A8D-600F-42E4-A72C-5B675BD60BAC")),
                            new(typeof(ManagementUnit), new Guid("77E78AEF-9512-46E0-A33D-AAE58DC7E18C")),
                        }));

    public static IServiceCollection RegisterSupportLegacyServices(this IServiceCollection services)
    {
        services.AddSingleton<ISecurityRuleParser, SampleSystemSecurityRuleParser>();

        //Custom ariba sender
        services.AddScoped<SampleSystemCustomAribaLocalDBEventMessageSender>();

        //For dto mapping
        services.AddScoped<ISampleSystemDTOMappingService, SampleSystemServerPrimitiveDTOMappingService>();

        //For mapping domain objects to dto events
        services
            .AddScoped<IDomainEventDTOMapper<PersistentDomainObjectBase>, RuntimeDomainEventDTOMapper<PersistentDomainObjectBase,
                ISampleSystemDTOMappingService, EventDTOBase>>();

        // For notification
        services.AddSingleton<IDefaultMailSenderContainer>(new DefaultMailSenderContainer("SampleSystem_Sender@luxoft.com"));
        services.AddScoped<IEmployeeSource, EmployeeSource<Employee>>();

        // For subscription
        services.AddSingleton(new SubscriptionMetadataFinderAssemblyInfo(typeof(EmployeeUpdateSubscription).Assembly));

        // For legacy audit
        services.AddKeyedSingleton("DTO", TypeResolverHelper.Create(TypeSource.FromSample<BusinessUnitSimpleDTO>(), TypeSearchMode.Both));

        // For SL
        services.AddSingleton<ISlJsonCompatibilitySerializer, SlJsonCompatibilitySerializer>();

        return services;
    }
}
