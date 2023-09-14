using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;
using Framework.SecuritySystem.Providers.Operation;

namespace Framework.SecuritySystem;

public static class SecurityPathExtensions
{
    public static ISecurityProvider<TDomainObject> ToProvider<TPersistentDomainObjectBase, TDomainObject, TIdent>(
        this SecurityPath<TDomainObject> securityPath,
        ContextSecurityOperation operation,
        ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory)

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        return new ContextSecurityPathProvider<TPersistentDomainObjectBase, TDomainObject, TIdent>(
            securityPath,
            operation,
            securityExpressionBuilderFactory);
    }
}
