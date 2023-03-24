using System;
using Framework.Persistent;

namespace Framework.DomainDriven.DAL.Revisions;

public class DomainObjectRevisionInfo<TIdent> : RevisionInfoBase, IDetail<DomainObjectRevision<TIdent>>
{
    private readonly DomainObjectRevision<TIdent> _master;

    public DomainObjectRevisionInfo(DomainObjectRevision<TIdent> master,  AuditRevisionType revisionType, string author, DateTime date, long revisionNumber) : base(revisionType, author, date, revisionNumber)
    {
        if (master == null) throw new ArgumentNullException(nameof(master));
        this._master = master;
        master.AddDetail(this);
    }

    DomainObjectRevision<TIdent> IDetail<DomainObjectRevision<TIdent>>.Master { get { return this._master; } }
}
