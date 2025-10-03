using Framework.Persistent;
using Framework.Validation;

namespace Framework.DomainDriven.BLL;

public interface IDefaultBLLContext<in TPersistentDomainObjectBase, TIdent> : IBLLBaseContext,

    IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>,

    IHierarchicalObjectExpanderFactoryContainer,

    IValidatorContainer

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>;
