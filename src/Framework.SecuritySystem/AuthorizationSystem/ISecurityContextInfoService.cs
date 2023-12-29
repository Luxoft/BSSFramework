namespace Framework.SecuritySystem;

public interface ISecurityContextInfoService
{
    ISecurityContextInfo GetSecurityContextInfo(Type type);

    ISecurityContextInfo GetSecurityContextInfo(string name);
}
