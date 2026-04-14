using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.NhFluentMapping;

namespace SampleSystem.Domain.TestDependency;

[BLLViewRole]
public class TestRootSecurityObj : BaseDirectory, IMaster<TestSecurityObjItem>
{
    private BusinessUnit businessUnit;

    private Location location;

    private readonly ICollection<TestSecurityObjItem> items = new List<TestSecurityObjItem>();

    private ManagementUnitFluentMapping managementUnitFluentMapping;

    public virtual IEnumerable<TestSecurityObjItem> Items => this.items;

    public virtual BusinessUnit BusinessUnit
    {
        get => this.businessUnit;
        set => this.businessUnit = value;
    }

    public virtual ManagementUnitFluentMapping ManagementUnitFluentMapping
    {
        get => this.managementUnitFluentMapping;
        set => this.managementUnitFluentMapping = value;
    }

    public virtual Location Location
    {
        get => this.location;
        set => this.location = value;
    }

    ICollection<TestSecurityObjItem> IMaster<TestSecurityObjItem>.Details => (ICollection<TestSecurityObjItem>)this.Items;
}
