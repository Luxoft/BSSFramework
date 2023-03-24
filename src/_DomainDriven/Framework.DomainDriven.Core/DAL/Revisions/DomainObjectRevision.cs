namespace Framework.DomainDriven.DAL.Revisions;

public class DomainObjectRevision<TIdent> : DomainObjectRevisionBase<TIdent, DomainObjectRevisionInfo<TIdent>>
{
    public DomainObjectRevision(TIdent identity)
            : base(identity)
    {
    }
}
