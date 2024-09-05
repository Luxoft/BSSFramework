namespace Framework.SecuritySystem;

public interface ISecurityContextSource
{
    IReadOnlyList<SecurityContextInfo> SecurityContextInfoList { get; }

    SecurityContextInfo GetSecurityContextInfo(Type securityContextType);

    SecurityContextInfo GetSecurityContextInfo(Guid securityContextTypeId);
}
