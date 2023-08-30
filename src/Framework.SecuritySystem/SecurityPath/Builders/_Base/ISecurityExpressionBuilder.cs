using Framework.Persistent;

namespace Framework.SecuritySystem.Rules.Builders;

public interface ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
    ISecurityExpressionFilter<TDomainObject> GetFilter<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
            where TSecurityOperationCode : struct, Enum;
}
