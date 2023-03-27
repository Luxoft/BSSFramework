using Framework.Persistent;

namespace SampleSystem.Domain;

public class Example2 : AuditPersistentDomainObjectBase, IDetail<Example1>
{
    private readonly Example1 parent;

    private Guid field1;

    private Guid field2;

    protected Example2()
    {
    }

    public Example2(Example1 parent)
    {
        this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
        this.parent.AddDetail(this);
    }


    public virtual Example1 Parent => this.parent;

    public virtual Guid Field1
    {
        get { return this.field1; }
        set { this.field1 = value; }
    }

    public virtual Guid Field2
    {
        get { return this.field2; }
        set { this.field2 = value; }
    }

    Example1 IDetail<Example1>.Master => this.Parent;
}
