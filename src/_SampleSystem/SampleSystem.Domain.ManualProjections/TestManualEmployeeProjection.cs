using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent.Mapping;
using Framework.Projection;
using Framework.Security;

namespace SampleSystem.Domain.ManualProjections;

[BLLProjectionViewRole]
[Projection(typeof(Employee), ProjectionRole.Default)]
[DependencySecurity(typeof(Employee))]
[Table(Name = nameof(Employee))]
public class TestManualEmployeeProjection : PersistentDomainObjectBase
{
    private readonly string login;

    private readonly Guid? coreBusinessUnitId;

    public virtual string Login => this.login;

    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual Guid? CoreBusinessUnitId => this.coreBusinessUnitId;
}
