namespace Framework.SecuritySystem;

/// <summary>
/// Операция доступа
/// </summary>
public abstract record SecurityOperation(string Name) : ISecurityOperation
{
    public bool AdminHasAccess { get; init; } = true;

    public string Description { get; init; }

    public bool IsClient { get; init; }

    public SecurityOperation ApproveOperation { get; init; }

    public sealed override string ToString() => this.Name;


    public static DisabledSecurityOperation Disabled { get; } = new ();
}
