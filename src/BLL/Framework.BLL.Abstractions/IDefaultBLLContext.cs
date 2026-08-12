using Anch.HierarchicalExpand;

using Framework.Application.Domain;

namespace Framework.BLL;

public interface IDefaultBLLContext<in TPersistentDomainObjectBase, TIdent> : IBLLBaseContext,

                                                                              IBLLFactoryContainerContext<IBLLFactoryContainer<
                                                                              IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    IHierarchicalObjectExpanderFactory HierarchicalObjectExpanderFactory { get; }
}
