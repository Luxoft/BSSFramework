using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem.Rules.Builders.Mixed
{
    public class SecurityExpressionFilter<TPersistentDomainObjectBase, TDomainObject, TIdent> : ISecurityExpressionFilter<TDomainObject>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        private readonly ISecurityExpressionFilter<TDomainObject> firstFilter;

        private readonly ISecurityExpressionFilter<TDomainObject> secondFilter;

        public SecurityExpressionFilter(
                [NotNull] ISecurityExpressionFilter<TDomainObject> firstFilter,
                [NotNull] ISecurityExpressionFilter<TDomainObject> secondFilter)
        {
            this.firstFilter = firstFilter ?? throw new ArgumentNullException(nameof(firstFilter));
            this.secondFilter = secondFilter ?? throw new ArgumentNullException(nameof(secondFilter));
        }

        public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc => this.secondFilter.InjectFunc;

        public Func<TDomainObject, bool> HasAccessFunc => this.firstFilter.HasAccessFunc;

        public IEnumerable<string> GetAccessors(TDomainObject domainObject)
        {
            return this.firstFilter.GetAccessors(domainObject);
        }
    }
}
