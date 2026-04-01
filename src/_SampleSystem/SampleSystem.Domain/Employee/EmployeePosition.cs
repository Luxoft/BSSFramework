using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
[UniqueGroup]
public class EmployeePosition : BaseDirectory, IExternalSynchronizable
{
    private long externalId;
    private string englishName;
    private Location location;

    public virtual long ExternalId
    {
        get => this.externalId;
        set => this.externalId = value;
    }

    [Required]
    [UniqueElement]
    public virtual Location Location
    {
        get => this.location;
        set => this.location = value;
    }

    [Required]
    [UniqueElement]
    public virtual string EnglishName
    {
        get => this.englishName.TrimNull();
        set => this.englishName = value.TrimNull();
    }
}
