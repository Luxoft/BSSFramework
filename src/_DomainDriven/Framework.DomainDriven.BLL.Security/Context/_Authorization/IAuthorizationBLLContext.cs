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

    bool HasAccess<TSecurityOperationCode>(NonContextSecurityOperation<TSecurityOperationCode> securityOperation, bool withRunAs)
        where TSecurityOperationCode : struct, Enum;

    void CheckAccess<TSecurityOperationCode>(NonContextSecurityOperation<TSecurityOperationCode> operation, bool withRunAs)
        where TSecurityOperationCode : struct, Enum;
}

public interface IAuthorizationBLLContext<TIdent> : IAuthorizationBLLContextBase, IAuthorizationSystem<TIdent>, IHierarchicalObjectExpanderFactoryContainer<TIdent>
{
}
