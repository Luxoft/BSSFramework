using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace SampleSystem.Domain;

[BLLViewRole]
public class TestSecuritySubObjItem2 : BaseDirectory, IDetail<TestSecurityObjItem>
{
    private TestSecurityObjItem innerMaster;

    protected TestSecuritySubObjItem2()
    {

    }

    public TestSecuritySubObjItem2(TestSecurityObjItem master)
    {
        if (master == null) throw new ArgumentNullException(nameof(master));

        this.innerMaster = master;
        this.innerMaster.AddDetail(this);
    }


    public virtual TestSecurityObjItem InnerMaster => this.innerMaster;

    [ExpandPath("InnerMaster.FirstMaster")]
    public virtual TestRootSecurityObj RootSecurityObj => this.InnerMaster.FirstMaster;

    TestSecurityObjItem IDetail<TestSecurityObjItem>.Master => this.InnerMaster;
}
