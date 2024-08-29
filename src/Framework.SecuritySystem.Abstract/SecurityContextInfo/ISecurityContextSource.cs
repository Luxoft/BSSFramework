namespace Framework.SecuritySystem;

public interface ISecurityContextSource
{
    IReadOnlyList<Type> SecurityContextTypes { get; }

    SecurityContextInfo GetSecurityContextInfo(Type type);

    SecurityContextInfo GetSecurityContextInfo(Guid ident);
}
