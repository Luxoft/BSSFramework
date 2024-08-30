﻿using System.Linq.Expressions;

using Framework.Persistent;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public abstract class ByIdentsFilterBuilder<TDomainObject, TSecurityContext> : SecurityFilterBuilder<TDomainObject>
    where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
{
    public sealed override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> permission)
    {
        if (permission.TryGetValue(typeof(TSecurityContext), out var securityIdents))
        {
            return this.GetSecurityFilterExpression(securityIdents);
        }
        else
        {
            return _ => true;
        }
    }

    protected abstract Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(IEnumerable<Guid> securityIdents);
}