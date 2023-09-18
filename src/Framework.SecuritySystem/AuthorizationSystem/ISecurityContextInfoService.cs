namespace Framework.SecuritySystem;

public interface ISecurityContextInfoService<TIdent>
{
    SecurityContextInfo<TIdent> GetSecurityContextInfo(Type type);
}
