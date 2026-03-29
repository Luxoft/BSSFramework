namespace Framework.Database.Domain;

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
