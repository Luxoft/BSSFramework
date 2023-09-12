//using System;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Linq.Expressions;

//using Framework.Core;
//using Framework.Persistent;


//namespace Framework.SecuritySystem.Rules.Builders.QueryableDenormalizedPermissions
//{
//    public class SecurityExpressionFilter<TPersistentDomainObjectBase, TDomainObject, TIdent> : ISecurityExpressionFilter<TDomainObject>

//        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
//        where TDomainObject : class, TPersistentDomainObjectBase
//        where TSecurityOperationCode : struct, Enum
//    {
//        private readonly Lazy<Expression<Func<TDomainObject, bool>>> optimizedLazyExpression;

//        private readonly Lazy<Func<TDomainObject, IEnumerable<string>>> getAccessorsFunc;

//        // ReSharper disable once StaticMemberInGenericType
//        [SuppressMessage("SonarQube", "S2743")]
//        private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache();

//        public SecurityExpressionFilter(
//            SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent> builder,
//            ContextSecurityOperation securityOperation)
//        {
//            if (builder == null) throw new ArgumentNullException(nameof(builder));
//            if (securityOperation == null) throw new
//                    ArgumentNullException(nameof(securityOperation));

//            var filterExpression = builder.GetSecurityFilterExpression(securityOperation).ExpandConst().InlineEval();

//            this.InjectFunc = q => q.Where(filterExpression);

//            this.optimizedLazyExpression = LazyHelper.Create(() => filterExpression.UpdateBody(OptimizeContainsCallVisitor<TIdent>.Value));

//            this.HasAccessFunc = filterExpression.Compile(LambdaCompileCache);

//            this.getAccessorsFunc = LazyHelper.Create(() => FuncHelper.Create((TDomainObject domainObject) =>
//            {
//                var baseFilter = builder.GetAccessorsFilterMany(domainObject, securityOperation.ExpandType);

//                var filter = baseFilter.OverrideInput((IPrincipal<TIdent> principal) => principal.Permissions);

//                return builder.Factory.AuthorizationSystem.GetAccessors(securityOperation.Code, filter);
//            }));
//        }


//        public Expression<Func<TDomainObject, bool>> Expression => this.optimizedLazyExpression.Value;

//        public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc { get; }

//        public Func<TDomainObject, bool> HasAccessFunc { get; }


//        public IEnumerable<string> GetAccessors(TDomainObject domainObject)
//        {
//            return this.getAccessorsFunc.Value(domainObject);
//        }
//    }
//}
