using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem;

public class DomainObjectIdentResolver<TIdent> : IDomainObjectIdentResolver
{
    public object TryGetIdent(object domainObject)
    {
        var identObj = domainObject as IIdentityObject<TIdent>;

        return identObj == null ? default(object) : identObj.Id;
    }

    public bool HasDefaultIdent(object domainObject)
    {
        var identObj = domainObject as IIdentityObject<TIdent>;

        return identObj is null || identObj.Id.IsDefault();
    }
}
