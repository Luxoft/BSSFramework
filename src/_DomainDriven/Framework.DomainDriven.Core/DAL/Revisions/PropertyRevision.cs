using System;
using Framework.Persistent;

namespace Framework.DomainDriven.DAL.Revisions;

public class PropertyRevision<TIdent, TProperty> : RevisionInfoBase, IDetail<DomainObjectPropertyRevisions<TIdent, TProperty>>
{
    private readonly TProperty _value;
    private readonly DomainObjectPropertyRevisions<TIdent, TProperty> _master;

    public PropertyRevision(DomainObjectPropertyRevisions<TIdent, TProperty> master, TProperty value, AuditRevisionType revisionType, string author, DateTime date, long revisionNumber)
            : base(revisionType, author, date, revisionNumber)
    {
        if (master == null) throw new ArgumentNullException(nameof(master));
        this._value = value;

        this._master = master;
        this._master.AddDetail(this);

    }

    public TProperty Value
    {
        get { return this._value; }
    }

    DomainObjectPropertyRevisions<TIdent, TProperty> IDetail<DomainObjectPropertyRevisions<TIdent, TProperty>>.Master { get { return this._master; } }
}
