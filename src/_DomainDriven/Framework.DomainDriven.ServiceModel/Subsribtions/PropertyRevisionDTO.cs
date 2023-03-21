using System;
using System.Runtime.Serialization;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven.ServiceModel.Subscriptions;

[DataContract]
public class PropertyRevisionDTOBase
{
    [DataMember] public AuditRevisionType RevisionType;
    [DataMember] public string Author;
    [DataMember] public DateTime Date;
    [DataMember] public long RevisionNumber;

    public PropertyRevisionDTOBase(RevisionInfoBase source)
    {
        this.RevisionType = source.RevisionType;
        this.Author = source.Author;
        this.Date = source.Date;
        this.RevisionNumber = source.RevisionNumber;
    }

    public PropertyRevisionDTOBase()
    {

    }
}

[DataContract(Name = "PropertyRevisionDTO{0}")]
public class PropertyRevisionDTO<TIdent> : PropertyRevisionDTOBase
{
    [DataMember]
    public string Value;

    public PropertyRevisionDTO() : base()
    {

    }

    public PropertyRevisionDTO(PropertyRevision<TIdent, string> source) : base(source)
    {
        this.Value = source.Value;

    }
}

[DataContract(Name = "PropertyRevisionDTO{0}{1}")]
public class PropertyRevisionDTO<TValue, TIdent> : PropertyRevisionDTOBase
{
    [DataMember]
    public TValue Value;

    public PropertyRevisionDTO()
            : base()
    {

    }

    public PropertyRevisionDTO(RevisionInfoBase source) : base(source)
    {

    }

    public PropertyRevisionDTO(PropertyRevision<TIdent, TValue> source)
            : base(source)
    {
        this.Value = source.Value;

    }
}
