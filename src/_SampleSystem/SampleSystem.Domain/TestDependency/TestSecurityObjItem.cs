using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Security;

namespace SampleSystem.Domain;

[BLLViewRole]
[DependencySecurity(typeof(TestRootSecurityObj), nameof(FirstMaster))]
public class TestSecurityObjItem : BaseDirectory, IDetail<TestRootSecurityObj>, IMaster<TestSecuritySubObjItem>, IMaster<TestSecuritySubObjItem2>, IMaster<TestSecuritySubObjItem3>
{
    private TestRootSecurityObj firstMaster;

    private readonly ICollection<TestSecuritySubObjItem> items = new List<TestSecuritySubObjItem>();

    private readonly ICollection<TestSecuritySubObjItem2> items2 = new List<TestSecuritySubObjItem2>();

    private readonly ICollection<TestSecuritySubObjItem3> items3 = new List<TestSecuritySubObjItem3>();


    protected TestSecurityObjItem()
    {
    }

    public TestSecurityObjItem(TestRootSecurityObj master)
    {
        if (master == null) throw new ArgumentNullException(nameof(master));

        this.firstMaster = master;
        this.firstMaster.AddDetail(this);
    }


    public virtual TestRootSecurityObj FirstMaster => this.firstMaster;

    public virtual IEnumerable<TestSecuritySubObjItem> Items => this.items;

    public virtual IEnumerable<TestSecuritySubObjItem2> Items2 => this.items2;

    public virtual IEnumerable<TestSecuritySubObjItem3> Items3 => this.items3;

    ICollection<TestSecuritySubObjItem> IMaster<TestSecuritySubObjItem>.Details => (ICollection<TestSecuritySubObjItem>)this.Items;

    ICollection<TestSecuritySubObjItem2> IMaster<TestSecuritySubObjItem2>.Details => (ICollection<TestSecuritySubObjItem2>)this.Items2;

    ICollection<TestSecuritySubObjItem3> IMaster<TestSecuritySubObjItem3>.Details => (ICollection<TestSecuritySubObjItem3>)this.Items3;

    TestRootSecurityObj IDetail<TestRootSecurityObj>.Master => this.FirstMaster;
}
