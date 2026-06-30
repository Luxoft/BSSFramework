using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;

namespace SampleSystem.Domain.TestDependency;

[BLLViewRole]
public class TestSecuritySubObjItem : BaseDirectory, IDetail<TestSecurityObjItem>
{
    private TestSecurityObjItem innerMaster = null!;

    protected TestSecuritySubObjItem()
    {

    }

    public TestSecuritySubObjItem(TestSecurityObjItem master)
    {
        this.innerMaster = master ?? throw new ArgumentNullException(nameof(master));
        this.innerMaster.AddDetail(this);
    }


    public virtual TestSecurityObjItem InnerMaster => this.innerMaster;

    TestSecurityObjItem IDetail<TestSecurityObjItem>.Master => this.InnerMaster;
}

