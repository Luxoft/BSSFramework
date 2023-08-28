using Framework.Persistent;

namespace Framework.SecuritySystem.Rules.Builders.Mixed;

public class SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> : ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    private readonly ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> hasAccessFactory;

    private readonly ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> queryFactory;

    public SecurityExpressionBuilderFactory(
            ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> hasAccessFactory,
            ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> queryFactory)
    {
        this.hasAccessFactory = hasAccessFactory ?? throw new ArgumentNullException(nameof(hasAccessFactory));
        this.queryFactory = queryFactory ?? throw new ArgumentNullException(nameof(queryFactory));
    }

    public ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent> path)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        var hasAccessBuilder = this.hasAccessFactory.CreateBuilder(path);
        var queryBuilder = this.queryFactory.CreateBuilder(path);

        return new SecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>(hasAccessBuilder, queryBuilder);
    }
}
