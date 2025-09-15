using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;
using Framework.Exceptions;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL;

public static class DefaultDomainBLLBaseExtensions
{
    public static TDomainObject GetById<TPersistentDomainObjectBase, TDomainObject>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, Guid> bll, string id)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

        return bll.GetById(Guid.Parse(id), true);
    }

    public static TDomainObject GetByIdOrCreate<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, TIdent id)
            where TDomainObject : class, TPersistentDomainObjectBase, new()
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));

        return bll.GetByIdOrCreate(id, () => new TDomainObject());
    }

    public static TDomainObject GetByIdOrCreate<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, TIdent id, Func<TDomainObject> createFunc)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        return bll.GetById(id, IdCheckMode.SkipEmpty) ?? createFunc();
    }

    public static TDomainObject GetByName<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, string name, bool throwOnNotFound, IFetchContainer<TDomainObject> fetchs)
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            where TDomainObject : class, TPersistentDomainObjectBase, IVisualIdentityObject
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));

        Expression<Func<TDomainObject, bool>> filter = v => v.Name == name;

        var result = bll.GetListBy(filter, fetchs).FirstOrDefault();

        if (null == result && throwOnNotFound)
        {
            throw new ObjectByNameNotFoundException(typeof(TDomainObject), name);
        }

        return result;
    }

    public static TDomainObject GetByName<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, string name, bool throwOnNotFound = false, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] fetchs)
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            where TDomainObject : class, TPersistentDomainObjectBase, IVisualIdentityObject
    {
        return bll.GetByName(name, throwOnNotFound, fetchs.ToFetchContainer());
    }

    public static TDomainObject GetByCode<TPersistentDomainObjectBase, TDomainObject, TIdent, TCode>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, TCode code, bool throwOnNotFound, IFetchContainer<TDomainObject> fetchs)
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            where TDomainObject : class, TPersistentDomainObjectBase, ICodeObject<TCode>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        
        var filter = EqualsHelper<TCode>.GetEqualsExpression(code).OverrideInput((TDomainObject domainObject) => domainObject.Code);

        var result = bll.GetListBy(filter, fetchs).FirstOrDefault();

        if (null == result && throwOnNotFound)
        {
            throw new ObjectByCodeNotFoundException<TCode>(typeof(TDomainObject), code);
        }

        return result;
    }

    public static TDomainObject GetByCode<TPersistentDomainObjectBase, TDomainObject, TIdent, TCode>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, TCode code, bool throwOnNotFound = false, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] fetchs)
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            where TDomainObject : class, TPersistentDomainObjectBase, ICodeObject<TCode>
    {
        return bll.GetByCode(code, throwOnNotFound, fetchs.ToFetchContainer());
    }

    public static TDomainObject GetByDomainType<TDomainObject>(this IBLLQueryBase<TDomainObject> bll, IDomainType domainType, bool throwOnNotFound = true)
            where TDomainObject : class, IDomainType
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var result = bll.GetObjectBy(domainObject => domainObject.Name == domainType.Name && domainObject.NameSpace == domainType.NameSpace);

        if (null == result && throwOnNotFound)
        {
            throw new BusinessLogicException($"{typeof(TDomainObject).Name} with name = {domainType.Name} and namespace = {domainType.NameSpace} not found");
        }

        return result;
    }

    public static void Insert<TDomainObject, TIdent>(this IDefaultDomainBLLBase<TDomainObject, TIdent> bll, TDomainObject domainObject)
            where TDomainObject : class, IIdentityObject<TIdent>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        bll.Insert(domainObject, domainObject.Id);
    }

    public static void Insert<TDomainObject, TIdent>(this IDefaultDomainBLLBase<TDomainObject, TIdent> bll, IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, IIdentityObject<TIdent>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

        domainObjects.Foreach(bll.Insert);
    }

    public static void SaveOrInsert<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, TDomainObject domainObject)
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        if (domainObject.Id.IsDefault())
        {
            bll.Save(domainObject);
        }
        else
        {
            bll.Insert(domainObject, domainObject.Id);
        }
    }
}
