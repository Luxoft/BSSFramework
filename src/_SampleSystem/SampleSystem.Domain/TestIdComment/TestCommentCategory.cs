namespace SampleSystem.Domain.TestIdComment;

public class TestCommentCategory : AuditPersistentDomainObjectBase
{
    private string name = null!;

    public virtual string Name
    {
        get => this.name;
        set => this.name = value;
    }
}
