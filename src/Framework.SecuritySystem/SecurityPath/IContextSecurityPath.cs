namespace Framework.SecuritySystem;

public interface IContextSecurityPath
{
    Type SecurityContextType { get; }

    string? Key { get; }
}
