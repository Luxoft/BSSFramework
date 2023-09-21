using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Tracking.LegacyValidators;
using Framework.Security;

namespace SampleSystem.Domain;

[BLLViewRole]
[BLLSaveRole]
[BLLIntegrationSaveRole]
[ViewDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.Disabled))]
[EditDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.Disabled))]
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
