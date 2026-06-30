using Framework.BLL.Domain.ServiceRole;
using Framework.Tracking.Validation;

namespace SampleSystem.Domain;

[BLLViewRole]
[BLLSaveRole]
[BLLIntegrationSaveRole]
public class TestImmutableObj : AuditPersistentDomainObjectBase
{
    private string testImmutablePrimitiveProperty = null!;

    private Employee.Employee testImmutableRefProperty = null!;

    [FixedPropertyValidator]
    public virtual string TestImmutablePrimitiveProperty
    {
        get => this.testImmutablePrimitiveProperty;
        set => this.testImmutablePrimitiveProperty = value;
    }

    [FixedPropertyValidator]
    public virtual Employee.Employee TestImmutableRefProperty
    {
        get => this.testImmutableRefProperty;
        set => this.testImmutableRefProperty = value;
    }
}

