namespace Framework.SecuritySystem;

public interface ISecurityContextInfoService
{
    IReadOnlyList<Type> SecurityContextTypes { get; }

    ISecurityContextInfo GetSecurityContextInfo(Type type);
}

public interface ISecurityContextInfoService<TIdent> : ISecurityContextInfoService
{
    new ISecurityContextInfo<TIdent> GetSecurityContextInfo(Type type);

    ISecurityContextInfo<TIdent> GetSecurityContextInfo(TIdent ident);
}
