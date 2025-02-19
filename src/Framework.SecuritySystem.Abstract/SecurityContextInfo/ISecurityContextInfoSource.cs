namespace Framework.SecuritySystem;

public interface ISecurityContextInfoSource
{
    IReadOnlyList<SecurityContextInfo> SecurityContextInfoList { get; }

    SecurityContextInfo GetSecurityContextInfo(Type securityContextType);

    SecurityContextInfo GetSecurityContextInfo(Guid securityContextTypeId);
}
