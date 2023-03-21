using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Persistent;

namespace Framework.SecuritySystem;

public abstract class SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
    protected SecurityPathBase()
    {

    }


    public IEnumerable<Type> GetUsedTypes()
    {
        return this.GetInternalUsedTypes().Distinct();
    }

    protected internal abstract IEnumerable<Type> GetInternalUsedTypes();
}
