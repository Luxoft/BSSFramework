using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class SingleContextFilterBuilder<TDomainObject, TSecurityContext>(
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    ISecurityContextSource securityContextSource,
    SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    : ByIdentsFilterBuilder<TDomainObject, TSecurityContext>(hierarchicalObjectExpanderFactory, securityContextSource)
    where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
{
    protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject)
    {
        var securityObject = securityPath.SecurityPath.Eval(domainObject, LambdaCompileCache);

        if (securityObject != null)
        {
            yield return securityObject;
        }
    }

    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}
