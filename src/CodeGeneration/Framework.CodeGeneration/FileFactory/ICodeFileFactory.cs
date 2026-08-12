using System.CodeDom;

namespace Framework.CodeGeneration.FileFactory;

public interface ICodeFileFactory : ICodeFile, IDomainTypeContainer
{
    string Name { get; }

    CodeTypeReference CurrentReference { get; }
}

public interface ICodeFileFactory<out TFileType> : ICodeFileFactory, IFileTypeSource<TFileType>
{
    ICodeFileFactoryHeader<TFileType> Header { get; }
}

public interface IFileTypeSource<out TFileType>
{
    TFileType FileType { get; }
}
