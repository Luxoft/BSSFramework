using System.CodeDom;

using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.Configuration;

public interface ICodeTypeReferenceService<in TFileType>
{
    string GetTypeName(Type domainType, TFileType fileType);

    CodeTypeReference GetCodeTypeReference(Type? domainType, TFileType fileType);

    ICodeFileFactoryHeader? GetFileFactoryHeader(TFileType fileType, bool raiseIfNotFound = true);
}
