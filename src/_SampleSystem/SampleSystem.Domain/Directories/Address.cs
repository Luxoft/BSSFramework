using Framework.Core;
using Framework.Relations;
using Framework.Restriction;

using SampleSystem.Domain.Enums;

namespace SampleSystem.Domain.Directories;

public class Address : AuditPersistentDomainObjectBase, IDetail<LegalEntityBase>
{
    private readonly LegalEntityBase legalEntity = null!;
    private AddressType addressType;
    private string cityName = "";
    private Country countryName = null!;
    private string regionName = "";
    private string street = "";
    private string zip = "";

    public Address(LegalEntityBase legalEntity)
    {
        this.legalEntity = legalEntity ?? throw new ArgumentNullException(nameof(legalEntity));
        this.legalEntity.AddDetail(this);
    }

    public Address(Guid id, LegalEntityBase legalEntity)
            : this(legalEntity) =>
        this.Id = id;

    protected Address()
    {
    }

    public virtual LegalEntityBase LegalEntity => this.legalEntity;

    [Required]
    public virtual Country CountryName
    {
        get => this.countryName;
        set => this.countryName = value;
    }

    public virtual AddressType AddressType
    {
        get => this.addressType;
        set => this.addressType = value;
    }

    [MaxLength(100)]
    public virtual string RegionName
    {
        get => this.regionName;
        set => this.regionName = value;
    }

    [MaxLength(100)]
    public virtual string CityName
    {
        get => this.cityName;
        set => this.cityName = value;
    }

    [MaxLength(100)]
    public virtual string Zip
    {
        get => this.zip;
        set => this.zip = value;
    }

    [MaxLength(100)]
    public virtual string Street
    {
        get => this.street;
        set => this.street = value;
    }

    LegalEntityBase IDetail<LegalEntityBase>.Master => this.LegalEntity;
}
