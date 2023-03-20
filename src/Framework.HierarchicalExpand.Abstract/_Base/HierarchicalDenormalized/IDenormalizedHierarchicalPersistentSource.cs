using System;

namespace Framework.Persistent;

public interface IDenormalizedHierarchicalPersistentSource<TDenormalize, TAncestorChildLink, out TDomainObject, out TIdent> : IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
        where TDenormalize : IHierarchicalAncestorLink<TDomainObject, TAncestorChildLink, TIdent>
        where TDomainObject : IDenormalizedHierarchicalPersistentSource<TDenormalize, TAncestorChildLink, TDomainObject, TIdent>
        where TAncestorChildLink : IHierarchicalToAncestorOrChildLink<TDomainObject, TIdent>
{
}
