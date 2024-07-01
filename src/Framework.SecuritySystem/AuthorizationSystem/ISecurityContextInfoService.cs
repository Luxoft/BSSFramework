namespace Framework.SecuritySystem;

public interface ISecurityContextInfoService
{
    IReadOnlyList<Type> SecurityContextTypes { get; }

    ISecurityContextInfo GetSecurityContextInfo(Type type);

    ISecurityContextInfo GetSecurityContextInfo(string name);

    ISecurityContextInfo<TIdent> GetSecurityContextInfo<TIdent>(Type type) =>
        (ISecurityContextInfo<TIdent>)this.GetSecurityContextInfo(type);

    ISecurityContextInfo<TIdent> GetSecurityContextInfo<TIdent>(string name) =>
        (ISecurityContextInfo<TIdent>)this.GetSecurityContextInfo(name);
}
