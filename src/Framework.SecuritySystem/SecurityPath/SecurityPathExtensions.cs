using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;
using Framework.SecuritySystem.Providers.Operation;

namespace Framework.SecuritySystem;

public static class SecurityPathExtensions
{
    public static ISecurityProvider<TDomainObject> ToProvider<TSecurityOperationCode, TPersistentDomainObjectBase, TDomainObject, TIdent>(
        this SecurityPath<TDomainObject> securityPath,
        ContextSecurityOperation<TSecurityOperationCode> operation,
        ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory)

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TSecurityOperationCode : struct, Enum
    {
        return new ContextSecurityPathProvider<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode>(
            securityPath,
            operation,
            securityExpressionBuilderFactory);
    }
}
