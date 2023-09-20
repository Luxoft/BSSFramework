using System.Linq.Expressions;

using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;

namespace Framework.SecuritySystem;

/// <summary>
/// Сервис с кешированием доступа к контекстным операциям
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
/// <typeparam name="TIdent"></typeparam>
public abstract class ContextDomainSecurityServiceBase<TDomainObject, TIdent> : NonContextDomainSecurityService<TDomainObject, TIdent>

    where TDomainObject : class, IIdentityObject<TIdent>
{
    private readonly ISecurityExpressionBuilderFactory securityExpressionBuilderFactory;

    protected ContextDomainSecurityServiceBase(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver securityOperationResolver,
        IAuthorizationSystem<TIdent> authorizationSystem,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)

        : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem)
    {
        this.securityExpressionBuilderFactory = securityExpressionBuilderFactory ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation securityOperation)
    {
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        switch (securityOperation)
        {
            case ContextSecurityOperation contextSecurityOperation:
                return this.CreateSecurityProvider(contextSecurityOperation);

            default:
                return base.CreateSecurityProvider(securityOperation);
        }
    }

    protected ISecurityProvider<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath, ContextSecurityOperation securityOperation)
        where TSecurityContext : class, ISecurityContext
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return this.Create(SecurityPath<TDomainObject>.Create(securityPath), securityOperation);
    }

    protected ISecurityProvider<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ContextSecurityOperation securityOperation)
        where TSecurityContext : class, ISecurityContext
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return this.Create(SecurityPath<TDomainObject>.Create(securityPath), securityOperation);
    }

    protected virtual ISecurityProvider<TDomainObject> Create(SecurityPath<TDomainObject> securityPath, ContextSecurityOperation securityOperation)
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return securityPath.ToProvider(securityOperation, this.securityExpressionBuilderFactory);
    }

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(ContextSecurityOperation securityOperation);
}

public class ContextDomainSecurityService<TDomainObject, TIdent> : ContextDomainSecurityServiceBase<TDomainObject, TIdent>

    where TDomainObject : class, IIdentityObject<TIdent>
{
    private readonly SecurityPath<TDomainObject> securityPath;

    public ContextDomainSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver securityOperationResolver,
        IAuthorizationSystem<TIdent> authorizationSystem,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
        SecurityPath<TDomainObject> securityPath)
        : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
    {
        this.securityPath = securityPath;
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(ContextSecurityOperation securityOperation)
    {
        return this.Create(this.securityPath, securityOperation);
    }
}
