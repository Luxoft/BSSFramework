using Framework.Persistent;
using Framework.Validation;

namespace Framework.DomainDriven.BLL;

public interface IDefaultBLLContext<in TPersistentDomainObjectBase, TIdent> : IBLLBaseContext,

    IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>,

    IHierarchicalObjectExpanderFactoryContainer<TIdent>,

    IValidatorContainer

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    /// <summary>
    /// Разрешение разворачивания доменного объекта вверх
    /// </summary>
    /// <typeparam name="TDomainObject">Доменный тип</typeparam>
    /// <returns></returns>
    bool AllowedExpandTreeParents<TDomainObject>()
        where TDomainObject : TPersistentDomainObjectBase;

}
