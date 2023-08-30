namespace Framework.Core;

public interface ITypeMap
{
    string Name { get; }
}

public interface ITypeMap<out TMember> : ITypeMap
        where TMember : ITypeMapMember
{
    IEnumerable<TMember> Members { get; }
}
