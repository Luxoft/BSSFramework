using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;
using Framework.Exceptions;
using Framework.Persistent;

using GenericQueryable.Fetching;

namespace Framework.DomainDriven.BLL;

public static class DefaultDomainBLLBaseExtensions
{
    public static TDomainObject GetByIdOrCreate<TPersistentDomainObjectBase, TDomainObject, TIdent>(
        this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll,
        TIdent id)
        where TDomainObject : class, TPersistentDomainObjectBase, new()
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));

        return bll.GetByIdOrCreate(id, () => new TDomainObject());
    }

    public static TDomainObject GetByIdOrCreate<TPersistentDomainObjectBase, TDomainObject, TIdent>(
        this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll,
        TIdent id,
        Func<TDomainObject> createFunc)
        where TDomainObject : class, TPersistentDomainObjectBase
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        return bll.GetById(id, IdCheckMode.SkipEmpty) ?? createFunc();
    }

    extension<TPersistentDomainObjectBase, TDomainObject, TIdent>(IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll)
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase, IVisualIdentityObject
    {
        [return: NotNullIfNotNull(nameof(throwOnNotFound))]
        public TDomainObject? GetByName(string name, bool throwOnNotFound, FetchRule<TDomainObject> fetchRule)
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
    }

    [return: NotNullIfNotNull(nameof(throwOnNotFound))]
    public static TDomainObject? GetByCode<TPersistentDomainObjectBase, TDomainObject, TIdent, TCode>(
        this IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll,
        TCode code,
        bool throwOnNotFound,
        FetchRule<TDomainObject> fetchRule)
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


    [return: NotNullIfNotNull(nameof(throwOnNotFound))]
    public static TDomainObject? GetByDomainType<TDomainObject>(this IBLLQueryBase<TDomainObject> bll, IDomainType domainType, bool throwOnNotFound = true)
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

    extension<TDomainObject, TIdent>(IDefaultDomainBLLBase<TDomainObject, TIdent> bll)
        where TDomainObject : class, IIdentityObject<TIdent>
    {
        public void Insert(TDomainObject domainObject)
        {
            if (bll == null) throw new ArgumentNullException(nameof(bll));
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            bll.Insert(domainObject, domainObject.Id);
        }

        public void Insert(IEnumerable<TDomainObject> domainObjects)
        {
            if (bll == null) throw new ArgumentNullException(nameof(bll));
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            domainObjects.Foreach(bll.Insert);
        }
    }

    public static void SaveOrInsert<TPersistentDomainObjectBase, TDomainObject, TIdent>(
        this IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> bll,
        TDomainObject domainObject)
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
