using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;

namespace Framework.SecuritySystem
{
    /// <summary>
    /// Контекстный провайдер доступа
    /// </summary>

    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TIdent"></typeparam>
    /// <typeparam name="TSecurityOperationCode"></typeparam>
    public class SecurityPathProvider<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode> : SecurityProvider<TDomainObject>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase

        where TSecurityOperationCode : struct, Enum
    {
        private readonly ContextSecurityOperation<TSecurityOperationCode> securityOperation;

        private readonly Lazy<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> injectFilterFunc;
        private readonly Lazy<ISecurityExpressionFilter<TDomainObject>> filterLazy;

        private readonly ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> securityExpressionBuilder;


        public SecurityPathProvider(
            IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService,
            SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent> securityPathBase,
            ContextSecurityOperation<TSecurityOperationCode> securityOperation,
            ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory)
            : base(accessDeniedExceptionService)
        {
            if (securityPathBase == null) throw new ArgumentNullException(nameof(securityPathBase));
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            this.securityOperation = securityOperation;

            this.securityExpressionBuilder = securityExpressionBuilderFactory.CreateBuilder(securityPathBase);

            this.filterLazy = LazyHelper.Create(() => this.securityExpressionBuilder.GetFilter(securityOperation));
            this.injectFilterFunc = LazyHelper.Create(() => this.filterLazy.Value.InjectFunc);
        }

        protected override LambdaCompileMode SecurityFilterCompileMode
        {
            get { return LambdaCompileMode.All; }
        }

        public override Expression<Func<TDomainObject, bool>> SecurityFilter
        {
            get { return this.filterLazy.Value.Expression; }
        }

        public override IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            return this.injectFilterFunc.Value(queryable);
        }


        public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return this.filterLazy.Value.GetAccessors(domainObject).ToUnboundedList();
        }

        public override Exception GetAccessDeniedException(TDomainObject domainObject, Func<string, string> formatMessageFunc = null)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return this.AccessDeniedExceptionService.GetAccessDeniedException(domainObject, new Dictionary<string, object> { { "operation", this.securityOperation.Code } }, formatMessageFunc);
        }
    }
}
