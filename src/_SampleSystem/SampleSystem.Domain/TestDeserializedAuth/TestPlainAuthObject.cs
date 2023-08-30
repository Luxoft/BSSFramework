using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace SampleSystem.Domain;

[BLLViewRole]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeeView)]
public class TestPlainAuthObject : BaseDirectory, IMaster<TestItemAuthObject>
{
    private readonly ICollection<TestItemAuthObject> items = new List<TestItemAuthObject>();

    private Location location;

    private Employee employee;


    public virtual Location Location
    {
        get { return this.location; }
        set { this.location = value; }
    }

    public virtual Employee Employee
    {
        get { return this.employee; }
        set { this.employee = value; }
    }

    public virtual IEnumerable<TestItemAuthObject> Items
    {
        get { return this.items; }
    }

    ICollection<TestItemAuthObject> IMaster<TestItemAuthObject>.Details => (ICollection<TestItemAuthObject>)this.Items;
}
