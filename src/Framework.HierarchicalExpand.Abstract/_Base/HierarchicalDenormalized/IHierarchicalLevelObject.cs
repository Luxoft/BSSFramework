namespace Framework.Persistent;

public interface IHierarchicalLevelObject
{
    int DeepLevel { get; }
}

public interface IModifiedIHierarchicalLevelObject : IHierarchicalLevelObject
{
    new int DeepLevel { get; set; }
}
