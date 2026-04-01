using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;

namespace SampleSystem.Domain;

[BLLViewRole]
public class TestPlainAuthObject : BaseDirectory, IMaster<TestItemAuthObject>
{
    private readonly ICollection<TestItemAuthObject> items = new List<TestItemAuthObject>();

    private Location location;

    private Employee employee;


    public virtual Location Location
    {
        get => this.location;
        set => this.location = value;
    }

    public virtual Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }

    public virtual IEnumerable<TestItemAuthObject> Items => this.items;

    ICollection<TestItemAuthObject> IMaster<TestItemAuthObject>.Details => (ICollection<TestItemAuthObject>)this.Items;
}
