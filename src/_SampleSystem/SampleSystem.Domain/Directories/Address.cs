using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;

namespace SampleSystem.Domain;

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
            : this(legalEntity)
    {
        this.Id = id;
    }

    protected Address()
    {
    }

    public virtual LegalEntityBase LegalEntity
    {
        get { return this.legalEntity; }
    }

    [Required]
    public virtual Country CountryName
    {
        get { return this.countryName; }
        set { this.countryName = value; }
    }

    public virtual AddressType AddressType
    {
        get { return this.addressType; }
        set { this.addressType = value; }
    }

    [MaxLength(100)]
    public virtual string RegionName
    {
        get { return this.regionName.TrimNull(); }
        set { this.regionName = value.TrimNull(); }
    }

    [MaxLength(100)]
    public virtual string CityName
    {
        get { return this.cityName.TrimNull(); }
        set { this.cityName = value.TrimNull(); }
    }

    [MaxLength(100)]
    public virtual string Zip
    {
        get { return this.zip.TrimNull(); }
        set { this.zip = value.TrimNull(); }
    }

    [MaxLength(100)]
    public virtual string Street
    {
        get { return this.street.TrimNull(); }
        set { this.street = value.TrimNull(); }
    }

    LegalEntityBase IDetail<LegalEntityBase>.Master
    {
        get { return this.LegalEntity; }
    }
}
