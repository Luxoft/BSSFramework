using Framework.DomainDriven.BLL;
using Framework.Exceptions;
using Framework.Persistent;

namespace Framework.DomainDriven;

public class DTOMappingVersionService<TBLLContext, TAuditPersistentDomainObjectBase, TIdent, TVersion> : BLLContextContainer<TBLLContext>, IDTOMappingVersionService<TAuditPersistentDomainObjectBase, TVersion>
        where TBLLContext : class
        where TAuditPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TVersion : IEquatable<TVersion>
{
    public DTOMappingVersionService(TBLLContext context)
            : base(context)
    {
    }


    public TVersion GetVersion<TDomainObject>(TVersion mappingObjectVersion, TDomainObject domainObject)
            where TDomainObject : TAuditPersistentDomainObjectBase, IVersionObject<TVersion>
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        if (!mappingObjectVersion.Equals(domainObject.Version))
        {
            throw new StaleDomainObjectStateException(new Exception("Mapping Exception"), typeof(TDomainObject), domainObject.Id);
        }

        return mappingObjectVersion;
    }
}
