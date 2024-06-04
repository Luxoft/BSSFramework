#nullable enable

namespace Framework.SecuritySystem;

/// <summary>
/// Операция доступа
/// </summary>
public class SecurityOperation(string name)
{
    public string Name { get; } = name;

    public string? Description { get; init; } = null;

    public override string ToString() => this.Name;

    public override bool Equals(object? obj) => this.Equals(obj as SecurityOperation);

    protected bool Equals(SecurityOperation? other) => this.Name == other?.Name;

    public override int GetHashCode() => this.Name.GetHashCode();


    public static bool operator ==(SecurityOperation so1, SecurityOperation so2)
    {
        return so1.Equals(so2);
    }

    public static bool operator !=(SecurityOperation so1, SecurityOperation so2)
    {
        return !so1.Equals(so2);
    }
}
