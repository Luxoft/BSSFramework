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

    public override string ToString() => this.Name;
}
