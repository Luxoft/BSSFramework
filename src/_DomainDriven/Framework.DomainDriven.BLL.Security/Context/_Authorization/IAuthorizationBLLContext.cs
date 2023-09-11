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

    /// <summary>
    /// NonContext checking
    /// </summary>
    /// <typeparam name="TSecurityOperationCode"></typeparam>
    /// <param name="securityOperationCode"></param>
    /// <param name="withRunAs"></param>
    /// <returns></returns>
    bool HasAccess<TSecurityOperationCode>(TSecurityOperationCode securityOperationCode, bool withRunAs)
        where TSecurityOperationCode : struct, Enum;

    /// <summary>
    /// NonContext checking
    /// </summary>
    /// <typeparam name="TSecurityOperationCode"></typeparam>
    /// <param name="securityOperationCode"></param>
    /// <param name="withRunAs"></param>
    void CheckAccess<TSecurityOperationCode>(TSecurityOperationCode securityOperationCode, bool withRunAs)
        where TSecurityOperationCode : struct, Enum;
}

public interface IAuthorizationBLLContext<TIdent> : IAuthorizationBLLContextBase, IAuthorizationSystem<TIdent>, IHierarchicalObjectExpanderFactoryContainer<TIdent>
{
}
