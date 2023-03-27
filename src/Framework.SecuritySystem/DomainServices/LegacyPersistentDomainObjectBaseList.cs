using System;
using System.Collections.Generic;

namespace Framework.SecuritySystem;

public class LegacyPersistentDomainObjectBaseList : List<Type>
{
    public LegacyPersistentDomainObjectBaseList(params Type[] types)
        : base(types)
    {
    }
}
