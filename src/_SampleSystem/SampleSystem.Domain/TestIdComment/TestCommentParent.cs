using Framework.Relations;

namespace SampleSystem.Domain.TestIdComment;

public class TestCommentParent : AuditPersistentDomainObjectBase, IMaster<TestComment>
{
    private readonly ICollection<TestComment> comments = new List<TestComment>();

    private string name = null!;

    private TestCommentCategory category = null!;

    public virtual string Name
    {
        get => this.name;
        set => this.name = value;
    }

    public virtual TestCommentCategory Category
    {
        get => this.category;
        set => this.category = value;
    }

    public virtual IEnumerable<TestComment> Comments => this.comments;

    ICollection<TestComment> IMaster<TestComment>.Details => (ICollection<TestComment>)this.Comments;
}
