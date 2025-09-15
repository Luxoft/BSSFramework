using CommonFramework;

using Framework.Core;

namespace Framework.DomainDriven.Metadata;

public class UniqueIndexMetadata
{
    private readonly DomainTypeMetadata _domainTypeMetadata;
    private readonly string _name;
    private readonly IList<FieldMetadata> _fields;

    internal UniqueIndexMetadata(DomainTypeMetadata domainTypeMetadata, string name, IEnumerable<FieldMetadata> fields)
    {
        if (domainTypeMetadata == null) throw new ArgumentNullException(nameof(domainTypeMetadata));
        this._domainTypeMetadata = domainTypeMetadata;
        this._name = name;
        this._fields = fields.ToList();
    }

    public string Name
    {
        get { return this._name; }
    }

    public IEnumerable<FieldMetadata> Fields
    {
        get { return this._fields; }
    }

    public override int GetHashCode()
    {
        return this._name != null ? this._name.GetHashCode() : 0;
    }

    public string FriendlyName
    {
        get
        {
            if (this._name.IsNullOrWhiteSpace())
            {
                return "UIX_" + this._fields.Select(z => z.Name).OrderBy(z => z).Join("_") + this._domainTypeMetadata.DomainType.Name;
            }
            return this._name + "_" + this._domainTypeMetadata.DomainType.Name;
        }
    }

    protected bool Equals(UniqueIndexMetadata other)
    {
        return string.Equals(this._name, other._name) && this.Equals(this._fields, other._fields);
    }

    private bool Equals(IEnumerable<FieldMetadata> arg1, IEnumerable<FieldMetadata> arg2)
    {
        return arg1.OrderBy(z => z.Name).SequenceEqual(arg2.OrderBy(z => z.Name));
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;

        return this.Equals((UniqueIndexMetadata)obj);
    }
}
