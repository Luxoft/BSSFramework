using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.DAL.Revisions;

public class TypeInfoDescription : IDomainType
{
    public TypeInfoDescription()
    {
    }

    public TypeInfoDescription(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        this.Name = type.Name;
        this.NameSpace = type.Namespace;
    }

    public TypeInfoDescription(IDomainType domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        this.Name = domainType.Name;
        this.NameSpace = domainType.NameSpace;
    }


    public string NameSpace { get; set; }

    public string Name { get; set; }


    public override string ToString()
    {
        return this.NameSpace.IsNullOrWhiteSpace() ? this.Name : $"{this.NameSpace}.{this.Name}";
    }
}

public class ObjectModificationInfo<TIdent>
{
    public ObjectModificationInfo()
    {

    }

    public ObjectModificationInfo(TIdent identity, TypeInfoDescription typeInfo, ModificationType modificationType, long revision)
    {
        this.Identity = identity;
        this.ModificationType = modificationType;
        this.Revision = revision;
        this.TypeInfo = typeInfo;
    }


    public ModificationType ModificationType { get; set; }

    public TIdent Identity { get; set; }

    public TypeInfoDescription TypeInfo { get; set; }

    public long Revision { get; set; }
}
