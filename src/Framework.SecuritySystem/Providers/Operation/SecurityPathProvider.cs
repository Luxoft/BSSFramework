using Framework.Core;
using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;

namespace Framework.SecuritySystem;

/// <summary>
/// Контекстный провайдер доступа
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
/// <typeparam name="TDomainObject"></typeparam>
/// <typeparam name="TIdent"></typeparam>
/// <typeparam name="TSecurityOperationCode"></typeparam>
public class SecurityPathProvider<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode> : SecurityProviderBase<TDomainObject>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase

        where TSecurityOperationCode : struct, Enum
{
    private readonly ContextSecurityOperation<TSecurityOperationCode> securityOperation;

    private readonly Lazy<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> injectFilterFunc;
    private readonly Lazy<ISecurityExpressionFilter<TDomainObject>> lazyFilter;

    private readonly ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> securityExpressionBuilder;


    public SecurityPathProvider(
            IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService,
            SecurityPath<TDomainObject> securityPathBase,
            ContextSecurityOperation<TSecurityOperationCode> securityOperation,
            ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory)
            : base(accessDeniedExceptionService)
    {
        if (securityPathBase == null) throw new ArgumentNullException(nameof(securityPathBase));
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        this.securityOperation = securityOperation;

        this.securityExpressionBuilder = securityExpressionBuilderFactory.CreateBuilder(securityPathBase);

        this.lazyFilter = LazyHelper.Create(() => this.securityExpressionBuilder.GetFilter(securityOperation));
        this.injectFilterFunc = LazyHelper.Create(() => this.lazyFilter.Value.InjectFunc);
    }

    public override IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
    {
        if (queryable == null) throw new ArgumentNullException(nameof(queryable));

        return this.injectFilterFunc.Value(queryable);
    }

    public override bool HasAccess(TDomainObject domainObject)
    {
        return this.lazyFilter.Value.HasAccessFunc(domainObject);
    }

    public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        return this.lazyFilter.Value.GetAccessors(domainObject).ToUnboundedList();
    }

    public override Exception GetAccessDeniedException(TDomainObject domainObject, Func<string, string> formatMessageFunc = null)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        return this.AccessDeniedExceptionService.GetAccessDeniedException(domainObject, new Dictionary<string, object> { { "operation", this.securityOperation.Code } }, formatMessageFunc);
    }
}
