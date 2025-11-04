using Framework.Persistent;

namespace Framework.DomainDriven.DAL.Revisions;

public class DomainObjectRevisionInfo<TIdent> : RevisionInfoBase, IDetail<DomainObjectRevision<TIdent>>
{
    private readonly DomainObjectRevision<TIdent> master;

    public DomainObjectRevisionInfo(
        DomainObjectRevision<TIdent> master,
        AuditRevisionType revisionType,
        string author,
        DateTime date,
        long revisionNumber)
        : base(revisionType, author, date, revisionNumber)
    {
        this.master = master;
        master.AddDetail(this);
    }

    DomainObjectRevision<TIdent> IDetail<DomainObjectRevision<TIdent>>.Master => this.master;
}
