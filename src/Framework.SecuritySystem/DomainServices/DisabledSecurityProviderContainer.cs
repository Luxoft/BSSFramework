using System.Diagnostics.CodeAnalysis;

namespace Framework.SecuritySystem;

public class DisabledSecurityProviderContainer<TPersistentDomainObjectBase> : IDisabledSecurityProviderContainer<TPersistentDomainObjectBase>
{
    private readonly IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService;

    public DisabledSecurityProviderContainer(
            [NotNull]
            IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService)
    {
        this.accessDeniedExceptionService = accessDeniedExceptionService ?? throw new ArgumentNullException(nameof(accessDeniedExceptionService));
    }

    public ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return new DisabledSecurityProvider<TDomainObject>(this.accessDeniedExceptionService);
    }
}
