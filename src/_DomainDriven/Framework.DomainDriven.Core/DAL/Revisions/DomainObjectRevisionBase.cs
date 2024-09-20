using Framework.Persistent;

namespace Framework.DomainDriven.DAL.Revisions;

public abstract class DomainObjectRevisionBase<TIdent, TRevisionItems>(TIdent identity) : IMaster<TRevisionItems>
{
    private readonly ICollection<TRevisionItems> revisionInfos = new List<TRevisionItems>();

    public TIdent Identity => identity;

    ICollection<TRevisionItems> IMaster<TRevisionItems>.Details => this.revisionInfos;

    public IEnumerable<TRevisionItems> RevisionInfos => this.revisionInfos;
}
