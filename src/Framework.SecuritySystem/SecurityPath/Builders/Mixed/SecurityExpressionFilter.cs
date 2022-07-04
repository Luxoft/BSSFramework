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
        private readonly ISecurityExpressionFilter<TDomainObject> hasAccessFilter;

        private readonly ISecurityExpressionFilter<TDomainObject> queryFilter;

        public SecurityExpressionFilter(
                [NotNull] ISecurityExpressionFilter<TDomainObject> hasAccessFilter,
                [NotNull] ISecurityExpressionFilter<TDomainObject> queryFilter)
        {
            this.hasAccessFilter = hasAccessFilter ?? throw new ArgumentNullException(nameof(hasAccessFilter));
            this.queryFilter = queryFilter ?? throw new ArgumentNullException(nameof(queryFilter));
        }

        public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc => this.queryFilter.InjectFunc;

        public Func<TDomainObject, bool> HasAccessFunc => this.hasAccessFilter.HasAccessFunc;

        public IEnumerable<string> GetAccessors(TDomainObject domainObject)
        {
            return this.hasAccessFilter.GetAccessors(domainObject);
        }
    }
}
