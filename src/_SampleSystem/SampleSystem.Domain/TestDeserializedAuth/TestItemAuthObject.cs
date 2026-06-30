using Framework.Relations;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.MU;

namespace SampleSystem.Domain.TestDeserializedAuth;

public class TestItemAuthObject : AuditPersistentDomainObjectBase, IDetail<TestPlainAuthObject>
{
    private readonly TestPlainAuthObject master = null!;

    private BusinessUnit businessUnit = null!;

    private ManagementUnit managementUnit = null!;

    protected TestItemAuthObject()
    {
    }

    public TestItemAuthObject(TestPlainAuthObject master)
    {
        this.master = master ?? throw new ArgumentNullException(nameof(master));
        this.master.AddDetail(this);
    }

    public virtual TestPlainAuthObject Master => this.master;

    public virtual BusinessUnit BusinessUnit
    {
        get => this.businessUnit;
        set => this.businessUnit = value;
    }
    public virtual ManagementUnit ManagementUnit
    {
        get => this.managementUnit;
        set => this.managementUnit = value;
    }
}

