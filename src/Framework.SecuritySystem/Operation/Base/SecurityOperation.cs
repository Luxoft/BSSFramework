namespace Framework.SecuritySystem;

/// <summary>
/// Операция доступа
/// </summary>
public abstract record SecurityOperation (string Name)
{
    public override string ToString() => this.Name;
}
