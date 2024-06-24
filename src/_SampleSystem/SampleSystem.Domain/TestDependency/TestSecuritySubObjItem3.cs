using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace SampleSystem.Domain;

[BLLViewRole]
public class TestSecuritySubObjItem3 : BaseDirectory, IDetail<TestSecurityObjItem>
{
    private TestSecurityObjItem innerMaster;

    protected TestSecuritySubObjItem3()
    {
    }

    public TestSecuritySubObjItem3(TestSecurityObjItem master)
    {
        if (master == null) throw new ArgumentNullException(nameof(master));

        this.innerMaster = master;
        this.innerMaster.AddDetail(this);
    }


    public virtual TestSecurityObjItem InnerMaster => this.innerMaster;

    TestSecurityObjItem IDetail<TestSecurityObjItem>.Master => this.InnerMaster;
}
