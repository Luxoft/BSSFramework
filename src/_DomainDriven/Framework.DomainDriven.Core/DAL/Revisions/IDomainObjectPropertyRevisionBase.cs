namespace Framework.DomainDriven.DAL.Revisions;

public interface IDomainObjectPropertyRevisionBase<out TIdent, out TRevisionItems>
{
    string PropertyName { get; }

    TIdent Identity { get; }

    IEnumerable<TRevisionItems> RevisionInfos { get; }
}
