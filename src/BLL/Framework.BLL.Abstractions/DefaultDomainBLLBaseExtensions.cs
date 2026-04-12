using System.Linq.Expressions;

using CommonFramework;

using Framework.Application.Domain;
using Framework.BLL.Domain.Exceptions;
using Framework.BLL.Domain.Persistent.IdentityObject;
using Framework.Core;

using GenericQueryable;
using GenericQueryable.Fetching;

namespace Framework.BLL;

public static class DefaultDomainBLLBaseExtensions
{
    public static TDomainObject? GetByIdOrCreate<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, TIdent id)
            where TDomainObject : class, TPersistentDomainObjectBase, new()
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));

        return bll.GetByIdOrCreate(id, () => new TDomainObject());
    }

    public static TDomainObject? GetByIdOrCreate<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, TIdent id, Func<TDomainObject> createFunc)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        return bll.GetById(id, IdCheckMode.SkipEmpty) ?? createFunc();
    }

    public static TDomainObject? GetByName<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, string name, bool throwOnNotFound, FetchRule<TDomainObject> fetchRule)
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            where TDomainObject : class, TPersistentDomainObjectBase, IVisualIdentityObject
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));

        Expression<Func<TDomainObject, bool>> filter = v => v.Name == name;

        var result = bll.GetListBy(filter, fetchRule).FirstOrDefault();

        if (null == result && throwOnNotFound)
        {
            throw new ObjectByNameNotFoundException(typeof(TDomainObject), name);
        }

        return result;
    }

    public static TDomainObject? GetByName<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, string name, bool throwOnNotFound, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule)
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            where TDomainObject : class, TPersistentDomainObjectBase, IVisualIdentityObject =>
        bll.GetByName(name, throwOnNotFound, buildFetchRule.ToFetchRule());

    public static TDomainObject? GetByName<TPersistentDomainObjectBase, TDomainObject, TIdent>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, string name, bool throwOnNotFound = false)
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase, IVisualIdentityObject =>
        bll.GetByName(name, throwOnNotFound, FetchRule<TDomainObject>.Empty);

    public static TDomainObject? GetByCode<TPersistentDomainObjectBase, TDomainObject, TIdent, TCode>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, TCode code, bool throwOnNotFound, FetchRule<TDomainObject> fetchRule)
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            where TDomainObject : class, TPersistentDomainObjectBase, ICodeObject<TCode>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));

        var filter = EqualsHelper<TCode>.GetEqualsExpression(code).OverrideInput((TDomainObject domainObject) => domainObject.Code);

        var result = bll.GetListBy(filter, fetchRule).FirstOrDefault();

        if (null == result && throwOnNotFound)
        {
            throw new ObjectByCodeNotFoundException<TCode>(typeof(TDomainObject), code);
        }

        return result;
    }

    public static TDomainObject? GetByCode<TPersistentDomainObjectBase, TDomainObject, TIdent, TCode>(this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll, TCode code, bool throwOnNotFound, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule)
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            where TDomainObject : class, TPersistentDomainObjectBase, ICodeObject<TCode> =>
        bll.GetByCode(code, throwOnNotFound, buildFetchRule.ToFetchRule());

    public static TDomainObject? GetByCode<TPersistentDomainObjectBase, TDomainObject, TIdent, TCode>(
        this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll,
        TCode code, bool throwOnNotFound = false)
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase, ICodeObject<TCode> =>
        bll.GetByCode(code, throwOnNotFound, FetchRule<TDomainObject>.Empty);

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
