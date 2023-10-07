namespace Framework.SecuritySystem;

public interface ISecurityContextInfoService
{
    SecurityContextInfo GetSecurityContextInfo(Type type);

    SecurityContextInfo GetSecurityContextInfo(string name);
}
