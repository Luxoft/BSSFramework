using Framework.Application.Domain;

namespace Framework.BLL;

public interface IDefaultBLLContext<in TPersistentDomainObjectBase, TIdent> : IBLLBaseContext,

    IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>,

    IHierarchicalObjectExpanderFactoryContainer

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>;
