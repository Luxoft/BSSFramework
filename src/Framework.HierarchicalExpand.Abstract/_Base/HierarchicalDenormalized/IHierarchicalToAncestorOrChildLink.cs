namespace Framework.Persistent;

public interface IHierarchicalToAncestorOrChildLink<out TDomain, TIdent>
        where TDomain : IIdentityObject<TIdent>
{
    TDomain ChildOrAncestor { get; }

    TDomain Source { get; }
}
