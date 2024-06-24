namespace Framework.SecuritySystem;

public interface ISecurityContextInfoService
{
    IReadOnlyList<Type> SecurityContextTypes { get; }

    ISecurityContextInfo GetSecurityContextInfo(Type type);

    ISecurityContextInfo GetSecurityContextInfo(string name);
}
