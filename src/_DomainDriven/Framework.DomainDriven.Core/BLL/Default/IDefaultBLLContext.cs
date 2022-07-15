using System;

using Framework.Persistent;
using Framework.Validation;

namespace Framework.DomainDriven.BLL
{
    public interface IDefaultBLLContext<in TPersistentDomainObjectBase, TIdent> :

        IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>, IValidatorContainer, IServiceProviderContainer

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
    }

    public interface IDefaultBLLContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent> :

        IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>,

        IBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>,

        IHierarchicalObjectExpanderFactoryContainer<TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
    {
        /// <summary>
        /// Разрешение разворачивания доменного объекта вверх
        /// </summary>
        /// <typeparam name="TDomainObject">Доменный тип</typeparam>
        /// <returns></returns>
        bool AllowedExpandTreeParents<TDomainObject>()
            where TDomainObject : TPersistentDomainObjectBase;

    }

    public interface IDefaultHierarchicalBLLContext<in TPersistentDomainObjectBase, TIdent> : IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
    }
}
