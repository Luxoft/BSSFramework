namespace Framework.DomainDriven.Generation.Domain;

public interface ICodeFileFactoryHeader
{
    string RelativePath { get; }

    string GetName(Type? domainType);
}

public interface ICodeFileFactoryHeader<out TFileType> : ICodeFileFactoryHeader
{
    TFileType Type { get; }
}

public class CodeFileFactoryHeader<TFileType>(TFileType type, string relativePath, Func<Type?, string> getNameFunc) : ICodeFileFactoryHeader<TFileType>
{
    public CodeFileFactoryHeader(TFileType type, string relativePath, Func<Type?, Enum> getNameFunc)
        : this(type, relativePath, t => getNameFunc(t).ToString())
    {
    }

    public TFileType Type { get; } = type;

    public string RelativePath { get; } = relativePath;

    public string GetName(Type? domainType)
    {
        return getNameFunc(domainType);
    }

    public override string ToString()
    {
        return $"Type:{this.Type}";
    }
}
