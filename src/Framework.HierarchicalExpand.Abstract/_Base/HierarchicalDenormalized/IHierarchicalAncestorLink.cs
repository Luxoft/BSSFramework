namespace Framework.Persistent;

/// <summary>
/// f
/// </summary>
/// <typeparam name="TDomain">f</typeparam>
/// <typeparam name="TDomainToAncestorChild">f</typeparam>
/// <typeparam name="TIdent">f</typeparam>
public interface IHierarchicalAncestorLink<out TDomain, TDomainToAncestorChild, TIdent>
        where TDomain : IIdentityObject<TIdent>
        where TDomainToAncestorChild : IHierarchicalToAncestorOrChildLink<TDomain, TIdent>
{
    TDomain Ancestor { get; }

    TDomain Child { get; }
}

public interface IModifiedHierarchicalAncestorLink<TDomain, TDomainToAncestorChild, TIdent> : IHierarchicalAncestorLink<TDomain, TDomainToAncestorChild, TIdent>
        where TDomain : IIdentityObject<TIdent>
        where TDomainToAncestorChild : IHierarchicalToAncestorOrChildLink<TDomain, TIdent>
{
    new TDomain Ancestor { get; set; }

    new TDomain Child { get; set; }
}
