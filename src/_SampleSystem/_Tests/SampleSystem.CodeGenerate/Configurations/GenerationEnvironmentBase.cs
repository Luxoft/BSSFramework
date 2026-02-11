using System.Reflection;

using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;
using Framework.Projection.Environment;
using Framework.Security;

using SecuritySystem;

using SampleSystem.Domain;
using SampleSystem.Security;

namespace SampleSystem.CodeGenerate;

public abstract class GenerationEnvironmentBase : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase,
    AuditPersistentDomainObjectBase, Guid>
{
    public readonly IProjectionEnvironment MainProjectionEnvironment;

    public readonly IProjectionEnvironment LegacyProjectionEnvironment;

    protected GenerationEnvironmentBase()
        : base(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
    {
        this.MainProjectionEnvironment = this.CreateDefaultProjectionLambdaEnvironment(new SampleSystemProjectionSource());

        this.LegacyProjectionEnvironment = this.CreateDefaultProjectionLambdaEnvironment(
            new LegacySampleSystemProjectionSource(),
            this.GetCreateProjectionLambdaSetupParams(
                projectionSubNamespace: "LegacyProjections",
                useDependencySecurity: false));
    }

    public override bool IsHierarchical(Type type) => new[] { typeof(BusinessUnit) }.Contains(type);

    public override IReadOnlyList<Type> SecurityRuleTypeList { get; } = [typeof(SampleSystemSecurityOperation), typeof(SecurityRule)];

    protected override IEnumerable<Assembly> GetDomainObjectAssemblies()
    {
        return base.GetDomainObjectAssemblies().Concat([typeof(Employee).Assembly]);
    }

    protected override IEnumerable<IProjectionEnvironment> GetProjectionEnvironments()
    {
        yield return this.MainProjectionEnvironment;

        yield return this.LegacyProjectionEnvironment;

        yield return this.CreateManualProjectionLambdaEnvironment(
            typeof(Domain.ManualProjections.TestManualEmployeeProjection).Assembly);
    }

    public override IDomainTypeRootExtendedMetadata ExtendedMetadata { get; } =

        new DomainTypeRootExtendedMetadataBuilder()
            .Add<Employee>(
                b => b.AddProperty(
                          e => e.PersonalCellPhones,
                          pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePersonalCellPhoneView))
                                  .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePersonalCellPhoneEdit)))
                      .AddProperty(
                          e => e.Login,
                          pb => pb.AddAttribute(new ViewDomainObjectAttribute(SecurityRule.View))
                                  .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.EmployeeEdit)))
                      .AddProperty(
                          e => e.PersonalCellPhone,
                          pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePersonalCellPhoneView)))
                      .AddProperty(
                          e => e.Position,
                          pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePositionView))
                                  .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePositionEdit))))
            .Add<Example1>(
                b => b.AddProperty(
                    e => e.Field3,
                    pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.LocationView))
                            .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.LocationEdit))))
            .Add<HRDepartment>(
                b => b.AddProperty(
                    e => e.CompanyLegalEntity,
                    pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.CompanyLegalEntityView))
                            .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.CompanyLegalEntityEdit))));
}
