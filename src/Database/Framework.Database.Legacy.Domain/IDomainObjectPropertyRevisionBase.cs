namespace Framework.Database.Domain;

public interface IDomainObjectPropertyRevisionBase<out TIdent, out TRevisionItems>
{
    string PropertyName { get; }

    TIdent Identity { get; }

    IEnumerable<TRevisionItems> RevisionInfos { get; }
}
