using System;
using System.Collections.Generic;

namespace Framework.DomainDriven.DAL.Revisions;

public class DomainObjectPropertyRevisions<TIdent> : DomainObjectRevisionBase<TIdent, PropertyRevision<TIdent, string>>
{
    private readonly string _propertyName;

    public DomainObjectPropertyRevisions(TIdent identity, string propertyName)
            : base(identity)
    {
        this._propertyName = propertyName;
    }

    public string PropertyName
    {
        get { return this._propertyName; }
    }
}

public class DomainObjectPropertyRevisionsBase<TIdent> :  DomainObjectRevisionBase<TIdent, RevisionInfoBase>, IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase>
{
    public DomainObjectPropertyRevisionsBase(TIdent identity, string propertyName) : base(identity)
    {
        this.PropertyName = propertyName;
    }

    public string PropertyName { get; private set; }
}
