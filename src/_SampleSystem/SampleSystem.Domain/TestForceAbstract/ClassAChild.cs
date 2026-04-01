namespace SampleSystem.Domain.TestForceAbstract;

public class ClassAChild : PersistentDomainObjectBase
{
    private ClassA parent;

    private bool isFake;

    public virtual ClassA Parent
    {
        get => this.parent;
        set => this.parent = value;
    }

    public virtual bool IsFake
    {
        get => this.isFake;
        set => this.isFake = value;
    }
}
