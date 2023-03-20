using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Framework.Core;

namespace Framework.Persistent;

public static class IdentityObjectContainerExtensions
{
    public static TIdentityObject TryGetIdentity<TIdentityObject>(this IIdentityObjectContainer<TIdentityObject> source)
    {
        return source.Maybe(v => v.Identity);
    }
}
