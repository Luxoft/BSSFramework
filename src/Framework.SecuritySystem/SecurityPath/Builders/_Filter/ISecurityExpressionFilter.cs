using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.SecuritySystem.Rules.Builders
{
    public interface ISecurityExpressionFilter<TDomainObject>
    {
        Expression<Func<TDomainObject, bool>> Expression { get; }

        Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc { get; }

        IEnumerable<string> GetAccessors(TDomainObject domainObject);
    }
}