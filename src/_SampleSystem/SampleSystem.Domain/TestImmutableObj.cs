using Framework.DomainDriven.BLL;

namespace SampleSystem.Domain;

[BLLViewRole]
[BLLSaveRole]
[BLLIntegrationSaveRole]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.Disabled)]
[SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.Disabled)]
public class TestImmutableObj : AuditPersistentDomainObjectBase
{
    private string testImmutablePrimitiveProperty;

    private Employee testImmutableRefProperty;

    [FixedPropertyValidator]
    public virtual string TestImmutablePrimitiveProperty
    {
        get => this.testImmutablePrimitiveProperty;
        set => this.testImmutablePrimitiveProperty = value;
    }

    [FixedPropertyValidator]
    public virtual Employee TestImmutableRefProperty
    {
        get => this.testImmutableRefProperty;
        set => this.testImmutableRefProperty = value;
    }
}
