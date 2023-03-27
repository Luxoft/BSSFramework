using Framework.Persistent;

using JetBrains.Annotations;

namespace SampleSystem.Domain;

public class TestItemAuthObject : AuditPersistentDomainObjectBase, IDetail<TestPlainAuthObject>
{
    private readonly TestPlainAuthObject master;

    private BusinessUnit businessUnit;

    private ManagementUnit managementUnit;

    protected TestItemAuthObject()
    {
    }

    public TestItemAuthObject([NotNull] TestPlainAuthObject master)
    {
        this.master = master ?? throw new ArgumentNullException(nameof(master));
        this.master.AddDetail(this);
    }

    public virtual TestPlainAuthObject Master => this.master;

    public virtual BusinessUnit BusinessUnit
    {
        get { return this.businessUnit; }
        set { this.businessUnit = value; }
    }
    public virtual ManagementUnit ManagementUnit
    {
        get { return this.managementUnit; }
        set { this.managementUnit = value; }
    }
}
