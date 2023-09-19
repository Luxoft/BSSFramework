using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

/// <summary>
/// Авторизационный контекст
/// </summary>
public interface IAuthorizationBLLContextBase : IAuthorizationSystem
{
    IRunAsManager RunAsManager
    {
        get;
    }

    string CurrentPrincipalName
    {
        get;
    }

    bool HasAccess(NonContextSecurityOperation securityOperation, bool withRunAs);

    void CheckAccess(NonContextSecurityOperation securityOperation, bool withRunAs);
}

public interface IAuthorizationBLLContext<TIdent> : IAuthorizationBLLContextBase, IHierarchicalObjectExpanderFactoryContainer<TIdent>
{
}
