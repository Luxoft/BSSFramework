using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL.Security;

public interface ISecurityBLLContext<TPersistentDomainObjectBase, TIdent> :

        IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> SecurityExpressionBuilderFactory { get; }
}

/// <summary>
/// Констекст с безопасностью
/// </summary>
/// <typeparam name="TAuthorizationBLLContext"></typeparam>
public interface ISecurityBLLContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent> :

        IDefaultBLLContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>

        where TDomainObjectBase : class
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
{
}
