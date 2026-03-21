using Framework.Application.Domain;

using Framework.BLL.HierarchicalExpand;

namespace Framework.BLL;

public interface IDefaultBLLContext<in TPersistentDomainObjectBase, TIdent> : IBLLBaseContext,

    IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>,

    IHierarchicalObjectExpanderFactoryContainer

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>;
