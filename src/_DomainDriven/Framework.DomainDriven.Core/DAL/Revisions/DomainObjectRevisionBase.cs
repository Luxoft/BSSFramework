using System.Collections.Generic;
using Framework.Persistent;

namespace Framework.DomainDriven.DAL.Revisions;

public interface IDomainObjectPropertyRevisionBase<out TIdent, out TRevisionItems>
{
    string PropertyName { get; }
    TIdent Identity { get; }
    IEnumerable<TRevisionItems> RevisionInfos { get; }
}
public abstract class DomainObjectRevisionBase<TIdent, TRevisionItems> : IMaster<TRevisionItems>
{
    private readonly TIdent _identity;
    private readonly ICollection<TRevisionItems> _revisionInfos;

    protected DomainObjectRevisionBase(TIdent identity)
    {
        this._identity = identity;
        this._revisionInfos = new List<TRevisionItems>();
    }

    public TIdent Identity
    {
        get { return this._identity; }
    }

    ICollection<TRevisionItems> IMaster<TRevisionItems>.Details
    {
        get { return this._revisionInfos; }
    }

    public IEnumerable<TRevisionItems> RevisionInfos
    {
        get { return this._revisionInfos; }
    }
}
