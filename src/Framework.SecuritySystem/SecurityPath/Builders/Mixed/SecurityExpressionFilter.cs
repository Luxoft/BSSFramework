using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem.Rules.Builders.Mixed
{
    public class SecurityExpressionFilter<TPersistentDomainObjectBase, TDomainObject, TSecurityOperationCode, TIdent> : ISecurityExpressionFilter<TDomainObject>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TSecurityOperationCode : struct, Enum
    {
        private readonly ISecurityExpressionFilter<TDomainObject> v1Filter;

        private readonly ISecurityExpressionFilter<TDomainObject> v2Filter;

        public SecurityExpressionFilter(
                [NotNull] ISecurityExpressionFilter<TDomainObject> v1Filter,
                [NotNull] ISecurityExpressionFilter<TDomainObject> v2Filter)
        {
            this.v1Filter = v1Filter ?? throw new ArgumentNullException(nameof(v1Filter));
            this.v2Filter = v2Filter ?? throw new ArgumentNullException(nameof(v2Filter));
        }

        public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc => this.v2Filter.InjectFunc;

        public Func<TDomainObject, bool> HasAccessFunc => this.v1Filter.HasAccessFunc;

        public IEnumerable<string> GetAccessors(TDomainObject domainObject)
        {
            return this.v1Filter.GetAccessors(domainObject);
        }
    }
}
