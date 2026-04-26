using Anch.Core;

using Framework.BLL.Domain.IdentityObject;

namespace Framework.BLL.Domain.Extensions;

public static class IdentityObjectContainerExtensions
{
    public static TIdentityObject TryGetIdentity<TIdentityObject>(this IIdentityObjectContainer<TIdentityObject> source) => source.Maybe(v => v.Identity);
}
