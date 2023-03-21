using System;
using System.Collections.Generic;

using Framework.Persistent;
using Framework.Validation;

namespace Framework.DomainDriven.BLL;

public class BLLContextValidationExtendedData<TBLLContext, TPersistentDomainObjectBase, TIdent> : ValidationExtendedData
        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public BLLContextValidationExtendedData(IAvailableValues availableValues)
            : base(availableValues)
    {

    }


    protected override IEnumerable<object> GetItems()
    {
        foreach (var item in base.GetItems())
        {
            yield return item;
        }

        yield return new BLLContextTypeData(typeof(TBLLContext), typeof(TPersistentDomainObjectBase), typeof(TIdent));
    }
}

public static class BLLContextValidationExtendedDataExtensions
{
    public static BLLContextValidationExtendedData<TBLLContext, TPersistentDomainObjectBase, TIdent> ToBLLContextValidationExtendedData<TBLLContext, TPersistentDomainObjectBase, TIdent>(this IAvailableValues availableValues)
            where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        if (availableValues == null) throw new ArgumentNullException(nameof(availableValues));

        return new BLLContextValidationExtendedData<TBLLContext, TPersistentDomainObjectBase, TIdent>(availableValues);
    }
}
