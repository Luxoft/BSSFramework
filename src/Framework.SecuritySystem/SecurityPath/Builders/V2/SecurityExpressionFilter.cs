using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;


namespace Framework.SecuritySystem.Rules.Builders.V2
{
    public class SecurityExpressionFilter<TPersistentDomainObjectBase, TDomainObject, TSecurityOperationCode, TIdent> : ISecurityExpressionFilter<TDomainObject>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TSecurityOperationCode : struct, Enum
    {
        private readonly Lazy<Expression<Func<TDomainObject, bool>>> optimizedLazyExpression;

        private readonly Lazy<Func<TDomainObject, IEnumerable<string>>> getAccessorsFunc;

        public SecurityExpressionFilter(
            SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent> builder,
            ContextSecurityOperation<TSecurityOperationCode> securityOperation)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            var expression = builder.GetSecurityFilterExpression(securityOperation).ExpandEval();

            this.InjectFunc = q => q.Where(expression);

            this.optimizedLazyExpression = LazyHelper.Create(() => expression.UpdateBody(OptimizeContainsCallVisitor<TIdent>.Value));

            this.getAccessorsFunc = LazyHelper.Create(() => FuncHelper.Create((TDomainObject domainObject) =>
            {
                var baseFilter = builder.GetAccessorsFilterMany(domainObject, securityOperation.SecurityExpandType);

                var filter = baseFilter.OverrideInput((IPrincipal<TIdent> principal) => principal.Permissions);

                return builder.Factory.AuthorizationSystem.GetAccessors(securityOperation.Code, filter);
            }));
        }


        public Expression<Func<TDomainObject, bool>> Expression { get { return this.optimizedLazyExpression.Value; } }

        public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc { get; private set; }


        public IEnumerable<string> GetAccessors(TDomainObject domainObject)
        {
            return this.getAccessorsFunc.Value(domainObject);
        }
    }
}
