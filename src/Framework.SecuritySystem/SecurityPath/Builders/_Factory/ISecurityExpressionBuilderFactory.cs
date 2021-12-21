using Framework.Persistent;

namespace Framework.SecuritySystem.Rules.Builders
{
    public interface ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> CreateBuilder<TDomainObject>(SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent> path)
             where TDomainObject : class, TPersistentDomainObjectBase;
    }
}