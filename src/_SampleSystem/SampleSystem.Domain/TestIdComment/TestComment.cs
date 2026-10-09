using Framework.Relations;

namespace SampleSystem.Domain.TestIdComment;

public class TestComment : AuditPersistentDomainObjectBase, IDetail<TestCommentParent>
{
    private TestCommentParent parent = null!;

    private string text = null!;

    protected TestComment()
    {
    }

    public TestComment(TestCommentParent parent)
    {
        this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
        this.parent.AddDetail(this);
    }

    public virtual TestCommentParent Parent => this.parent;

    public virtual string Text
    {
        get => this.text;
        set => this.text = value;
    }

    TestCommentParent IDetail<TestCommentParent>.Master => this.Parent;
}
