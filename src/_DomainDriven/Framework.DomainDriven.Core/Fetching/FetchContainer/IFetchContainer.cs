using System;

using Framework.Core;

namespace Framework.DomainDriven;

public interface IFetchContainer<TDomainObject>
{
    IPropertyPathTree<TDomainObject> Fetchs { get; }
}
