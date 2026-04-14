using Framework.Core;
using Framework.Relations;
using Framework.Restriction;

using SampleSystem.Domain.Enums;

namespace SampleSystem.Domain.Directories;

public class Address : AuditPersistentDomainObjectBase, IDetail<LegalEntityBase>
{
    private readonly LegalEntityBase legalEntity;
    private AddressType addressType;
    private string cityName;
    private Country countryName;
    private string regionName;
    private string street;
    private string zip;

    public Address(LegalEntityBase legalEntity)
    {
        if (legalEntity == null) throw new ArgumentNullException(nameof(legalEntity));

        this.legalEntity = legalEntity;
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
        get => this.regionName.TrimNull();
        set => this.regionName = value.TrimNull();
    }

    [MaxLength(100)]
    public virtual string CityName
    {
        get => this.cityName.TrimNull();
        set => this.cityName = value.TrimNull();
    }

    [MaxLength(100)]
    public virtual string Zip
    {
        get => this.zip.TrimNull();
        set => this.zip = value.TrimNull();
    }

    [MaxLength(100)]
    public virtual string Street
    {
        get => this.street.TrimNull();
        set => this.street = value.TrimNull();
    }

    LegalEntityBase IDetail<LegalEntityBase>.Master => this.LegalEntity;
}
