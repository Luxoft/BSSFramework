namespace Framework.Restriction;

public interface IUniqueAttribute : IRestrictionAttribute
{
    string Key { get; }
}
