using System.Reflection;

using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Generation.Domain.ExtendedMetadata;
using Framework.Projection;
using Framework.Security;

using SampleSystem.Domain;

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

    public override Type SecurityOperationType { get; } = typeof(SampleSystemSecurityOperation);

    public override Type OperationContextType
    {
        get;
    } = typeof(SampleSystemOperationContext);

    protected override IEnumerable<Assembly> GetDomainObjectAssemblies()
    {
        return base.GetDomainObjectAssemblies().Concat(new[] { typeof(Employee).Assembly });
    }

    protected override IEnumerable<IProjectionEnvironment> GetProjectionEnvironments()
    {
        yield return this.MainProjectionEnvironment;

        yield return this.LegacyProjectionEnvironment;

        yield return this.CreateManualProjectionLambdaEnvironment(typeof(SampleSystem.Domain.ManualProjections.TestManualEmployeeProjection).Assembly);
    }

    public override IDomainTypeRootExtendedMetadata ExtendedMetadata { get; } =

        new DomainTypeRootExtendedMetadataBuilder()
            .Add<Employee>(
                b => b.AddProperty(
                    e => e.PersonalCellPhones,
                    pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePersonalCellPhoneView))
                            .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePersonalCellPhoneEdit))));
}
