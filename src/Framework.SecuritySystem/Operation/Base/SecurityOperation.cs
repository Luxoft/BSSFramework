namespace Framework.SecuritySystem;

/// <summary>
/// Операция доступа
/// </summary>
public abstract record SecurityOperation (string Name)
{
    public bool AdminHasAccess { get; init; } = true;

    public override string ToString() => this.Name;
}
