using Framework.BLL.Domain.Fetching;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;
using Framework.Restriction;

using SecuritySystem;

namespace SampleSystem.Domain;

[BLLViewRole, BLLSaveRole, BLLRemoveRole]
[UniqueGroup]
public class Location :
        BaseDirectory,
        IMaster<Location>,
        IDetail<Location>,
        ISecurityContext
{
    private readonly ICollection<Location> children = new List<Location>();

    private Country country;
    private bool isFinancial;
    private LocationType locationType;
    private Location? parent;

    private int closeDate;
    private int code;

    private byte[] binaryData;

    private int deepLevel;

    public Location()
    {
    }

    public Location(Location? parent)
    {
        if (parent != null)
        {
            this.parent = parent;
            this.parent.AddDetail(this);
        }
    }


    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual int DeepLevel
    {
        get => this.deepLevel;
        set => this.deepLevel = value;
    }

    public virtual byte[] BinaryData
    {
        get => this.binaryData;
        set => this.binaryData = value;
    }

    [FetchPath("Children")]
    public virtual bool IsLeaf => !this.Children.Any();

    [FetchPath("Children")]
    public virtual bool ContainsOnlyInactiveChildren => this.Children.All(x => !x.Active);

    public virtual Country? Country
    {
        get => this.country;
        set => this.country = value;
    }

    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual Location Root => this.Parent == null ? this : this.Parent.Root;

    public virtual LocationType LocationType
    {
        get => this.locationType;
        set => this.locationType = value;
    }

    public virtual bool IsFinancial
    {
        get => this.isFinancial;
        set => this.isFinancial = value;
    }

    [Required]
    public virtual int CloseDate
    {
        get => this.closeDate;
        set => this.closeDate = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual IEnumerable<Location> Children => this.children;

    public virtual Location? Parent
    {
        get => this.parent;
        set => this.parent = value;
    }

    [Required]
    public virtual int Code
    {
        get => this.code;
        set => this.code = value;
    }

    [CustomSerialization(CustomSerializationMode.Normal)]
    public override bool Active
    {
        get => base.Active;
        set => base.Active = value;
    }

    Location? IDetail<Location>.Master => this.Parent;

    ICollection<Location> IMaster<Location>.Details => (ICollection<Location>)this.Children;
}
