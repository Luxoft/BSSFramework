using System;
using System.ComponentModel;

using Framework.Persistent;
using Framework.QueryableSource;
using Framework.Validation;

namespace Framework.DomainDriven.BLL
{
    public interface IDefaultBLLContext<in TPersistentDomainObjectBase, TIdent> :

        IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>>, IValidatorContainer, IServiceProviderContainer

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        [Obsolete("Please don't use!")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        IQueryableSource<TPersistentDomainObjectBase> GetQueryableSource();
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
