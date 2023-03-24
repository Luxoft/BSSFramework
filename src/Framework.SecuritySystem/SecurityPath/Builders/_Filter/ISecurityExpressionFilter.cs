using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.SecuritySystem.Rules.Builders;

public interface ISecurityExpressionFilter<TDomainObject>
{
    Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc { get; }

    Func<TDomainObject, bool> HasAccessFunc { get; }

    IEnumerable<string> GetAccessors(TDomainObject domainObject);
}
