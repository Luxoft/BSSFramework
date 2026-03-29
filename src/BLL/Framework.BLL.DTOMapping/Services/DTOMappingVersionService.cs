using Framework.Application.Domain;
using Framework.BLL.Domain.IdentityObject;
using Framework.Database;

namespace Framework.BLL.DTOMapping.Services;

public class DTOMappingVersionService<TBLLContext, TAuditPersistentDomainObjectBase, TIdent, TVersion>(TBLLContext context)
    : BLLContextContainer<TBLLContext>(context), IDTOMappingVersionService<TAuditPersistentDomainObjectBase, TVersion>
    where TBLLContext : class
    where TAuditPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TVersion : IEquatable<TVersion>
{
    public TVersion GetVersion<TDomainObject>(TVersion mappingObjectVersion, TDomainObject domainObject)
            where TDomainObject : TAuditPersistentDomainObjectBase, IVersionObject<TVersion>
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        if (!mappingObjectVersion.Equals(domainObject.Version))
        {
            throw new StaleDomainObjectStateException(typeof(TDomainObject), domainObject.Id!, new Exception("Mapping Exception"));
        }

        return mappingObjectVersion;
    }
}
