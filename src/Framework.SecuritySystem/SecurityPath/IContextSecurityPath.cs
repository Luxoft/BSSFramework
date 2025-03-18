namespace Framework.SecuritySystem;

public interface IContextSecurityPath
{
    Type SecurityContextType { get; }

    bool Required { get; }

    string? Key { get; }
}
