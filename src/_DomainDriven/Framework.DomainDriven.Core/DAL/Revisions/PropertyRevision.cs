using Framework.Persistent;

namespace Framework.DomainDriven.DAL.Revisions;

public class PropertyRevision<TIdent, TProperty> : RevisionInfoBase, IDetail<DomainObjectPropertyRevisions<TIdent, TProperty>>
{
    private readonly TProperty value;
    private readonly DomainObjectPropertyRevisions<TIdent, TProperty> master;

    public PropertyRevision(DomainObjectPropertyRevisions<TIdent, TProperty> master, TProperty value, AuditRevisionType revisionType, string author, DateTime date, long revisionNumber)
            : base(revisionType, author, date, revisionNumber)
    {
        this.value = value;

        this.master = master;
        this.master.AddDetail(this);
    }

    public TProperty Value
    {
        get { return this.value; }
    }

    DomainObjectPropertyRevisions<TIdent, TProperty> IDetail<DomainObjectPropertyRevisions<TIdent, TProperty>>.Master { get { return this.master; } }
}
