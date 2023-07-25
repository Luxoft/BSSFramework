namespace Framework.Persistent;

public interface IModifiedHierarchicalAncestorLink<TDomain, TDomainToAncestorChild, TIdent> : IHierarchicalAncestorLink<TDomain, TDomainToAncestorChild, TIdent>
    where TDomain : IIdentityObject<TIdent>
    where TDomainToAncestorChild : IHierarchicalToAncestorOrChildLink<TDomain, TIdent>
{
    new TDomain Ancestor { get; set; }

    new TDomain Child { get; set; }
}
