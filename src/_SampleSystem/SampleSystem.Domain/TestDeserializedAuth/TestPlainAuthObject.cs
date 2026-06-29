using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;

using SampleSystem.Domain.Directories;

namespace SampleSystem.Domain.TestDeserializedAuth;

[BLLViewRole]
public class TestPlainAuthObject : BaseDirectory, IMaster<TestItemAuthObject>
{
    private readonly ICollection<TestItemAuthObject> items = new List<TestItemAuthObject>();

    private Location location = null!;

    private Employee.Employee employee = null!;


    public virtual Location Location
    {
        get => this.location;
        set => this.location = value;
    }

    public virtual Employee.Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }

    public virtual IEnumerable<TestItemAuthObject> Items => this.items;

    ICollection<TestItemAuthObject> IMaster<TestItemAuthObject>.Details => (ICollection<TestItemAuthObject>)this.Items;
}

