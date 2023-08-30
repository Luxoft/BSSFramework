﻿using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;

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
        return new SecurityPathProvider<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode>(
            securityPath,
            operation,
            securityExpressionBuilderFactory);
    }
}
