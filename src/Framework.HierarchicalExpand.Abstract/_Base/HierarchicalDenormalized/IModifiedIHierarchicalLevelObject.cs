namespace Framework.Persistent;

public interface IModifiedIHierarchicalLevelObject : IHierarchicalLevelObject
{
    new int DeepLevel { get; set; }
}
