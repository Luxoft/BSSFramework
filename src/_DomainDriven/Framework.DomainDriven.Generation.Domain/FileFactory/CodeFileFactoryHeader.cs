namespace Framework.DomainDriven.Generation.Domain;

public interface ICodeFileFactoryHeader
{
    string RelativePath { get; }

    string GetName(Type domainType);
}

public interface ICodeFileFactoryHeader<out TFileType> : ICodeFileFactoryHeader
{
    TFileType Type { get; }
}

public class CodeFileFactoryHeader<TFileType> : ICodeFileFactoryHeader<TFileType>
{
    private readonly Func<Type, string> _getNameFunc;


    public CodeFileFactoryHeader(TFileType type, string relativePath, Func<Type, string> getNameFunc)
    {
        this._getNameFunc = getNameFunc ?? throw new ArgumentNullException(nameof(getNameFunc));
        this.Type = type ?? throw new ArgumentNullException(nameof(type));
        this.RelativePath = relativePath ?? throw new ArgumentNullException(nameof(relativePath));
    }


    public TFileType Type { get; }

    public string RelativePath { get; }


    public string GetName(Type domainType)
    {
        return this._getNameFunc(domainType);
    }

    public override string ToString()
    {
        return $"Type:{this.Type}";
    }
}
