namespace Framework.SecuritySystem;

public interface ISecurityOperation
{
    string Name { get; }

    public bool AdminHasAccess { get; }

    public string Description { get; }

    public bool IsClient { get; }

    public SecurityOperation ApproveOperation { get; }
}
