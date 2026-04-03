using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;

namespace SampleSystem.Domain;

[BLLViewRole]
public class TestSecuritySubObjItem : BaseDirectory, IDetail<TestSecurityObjItem>
{
    private TestSecurityObjItem innerMaster;

    protected TestSecuritySubObjItem()
    {

    }

    public TestSecuritySubObjItem(TestSecurityObjItem master)
    {
        if (master == null) throw new ArgumentNullException(nameof(master));

        this.innerMaster = master;
        this.innerMaster.AddDetail(this);
    }


    public virtual TestSecurityObjItem InnerMaster => this.innerMaster;

    TestSecurityObjItem IDetail<TestSecurityObjItem>.Master => this.InnerMaster;
}
