using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.Database.Mapping;
using Framework.Projection;

namespace SampleSystem.Domain.ManualProjections;

[BLLProjectionViewRole]
[Projection(typeof(Employee.Employee), ProjectionRole.Default)]
[DependencySecurity(typeof(Employee.Employee))]
[Table(Name = nameof(Employee.Employee))]
public class TestManualEmployeeProjection : PersistentDomainObjectBase
{
    private readonly string login;

    private readonly Guid? coreBusinessUnitId;

    public virtual string Login => this.login;

    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual Guid? CoreBusinessUnitId => this.coreBusinessUnitId;
}
