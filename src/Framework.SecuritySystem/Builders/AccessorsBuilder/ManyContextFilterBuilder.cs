using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class ManyContextFilterBuilder<TDomainObject, TSecurityContext>(
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    ISecurityContextSource securityContextSource,
    SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    : ByIdentsFilterBuilder<TDomainObject, TSecurityContext>(hierarchicalObjectExpanderFactory, securityContextSource)
    where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
{
    protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject) =>
        securityPath.SecurityPath.Eval(domainObject, LambdaCompileCache).EmptyIfNull();

    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}
