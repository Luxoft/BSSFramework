using Anch.HierarchicalExpand;

using Framework.Application.Domain;

namespace Framework.BLL;

public interface IDefaultBLLContext : IBLLBaseContext
{
    IHierarchicalObjectExpanderFactory HierarchicalObjectExpanderFactory { get; }
}


public interface IDefaultBLLContext<in TPersistentDomainObjectBase, TIdent> : IDefaultBLLContext,

                                                                              IBLLFactoryContainerContext<IBLLFactoryContainer<
                                                                              IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>;
