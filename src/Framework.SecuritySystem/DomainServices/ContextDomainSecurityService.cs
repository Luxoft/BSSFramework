using System.Linq.Expressions;

using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;

namespace Framework.SecuritySystem;

/// <summary>
/// Сервис с кешированием доступа к контекстным операциям
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
/// <typeparam name="TIdent"></typeparam>
public abstract class ContextDomainSecurityServiceBase<TDomainObject, TIdent> : DomainSecurityService<TDomainObject>

    where TDomainObject : IIdentityObject<TIdent>
{
    private readonly ISecurityExpressionBuilderFactory securityExpressionBuilderFactory;

    protected ContextDomainSecurityServiceBase(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        ISecurityOperationResolver securityOperationResolver,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)

        : base(disabledSecurityProvider, securityOperationResolver)
    {
        this.securityExpressionBuilderFactory = securityExpressionBuilderFactory ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));
    }

    protected ISecurityProvider<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath, SecurityOperation securityOperation)
        where TSecurityContext : class, ISecurityContext
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return this.Create(SecurityPath<TDomainObject>.Create(securityPath), securityOperation);
    }

    protected ISecurityProvider<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, SecurityOperation securityOperation)
        where TSecurityContext : class, ISecurityContext
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return this.Create(SecurityPath<TDomainObject>.Create(securityPath), securityOperation);
    }

    protected virtual ISecurityProvider<TDomainObject> Create(SecurityPath<TDomainObject> securityPath, SecurityOperation securityOperation)
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return securityPath.ToProvider(securityOperation, this.securityExpressionBuilderFactory);
    }
}

public class ContextDomainSecurityService<TDomainObject, TIdent> : ContextDomainSecurityServiceBase<TDomainObject, TIdent>

    where TDomainObject : IIdentityObject<TIdent>
{
    private readonly SecurityPath<TDomainObject> securityPath;

    public ContextDomainSecurityService(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        ISecurityOperationResolver securityOperationResolver,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
        SecurityPath<TDomainObject> securityPath)
        : base(disabledSecurityProvider, securityOperationResolver, securityExpressionBuilderFactory)
    {
        this.securityPath = securityPath;
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation securityOperation)
    {
        return this.Create(this.securityPath, securityOperation);
    }
}
