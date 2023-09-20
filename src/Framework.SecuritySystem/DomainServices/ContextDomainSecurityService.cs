using System.Linq.Expressions;

using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;

namespace Framework.SecuritySystem;

/// <summary>
/// Сервис с кешированием доступа к контекстным операциям
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
/// <typeparam name="TDomainObject"></typeparam>
/// <typeparam name="TIdent"></typeparam>
public abstract class ContextDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TIdent> : NonContextDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
{
    private readonly ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory;

    protected ContextDomainSecurityServiceBase(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver<TPersistentDomainObjectBase> securityOperationResolver,
        IAuthorizationSystem<TIdent> authorizationSystem,
        ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory)

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
        where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return this.Create(SecurityPath<TDomainObject>.Create(securityPath), securityOperation);
    }

    protected ISecurityProvider<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ContextSecurityOperation securityOperation)
        where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
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

public class ContextDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TIdent> : ContextDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
{
    private readonly SecurityPath<TDomainObject> securityPath;

    public ContextDomainSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver<TPersistentDomainObjectBase> securityOperationResolver,
        IAuthorizationSystem<TIdent> authorizationSystem,
        ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory,
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
