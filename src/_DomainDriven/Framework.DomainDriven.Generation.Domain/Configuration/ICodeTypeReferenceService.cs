using System.CodeDom;

namespace Framework.DomainDriven.Generation.Domain;

public interface ICodeTypeReferenceService<in TFileType>
{
    string GetTypeName(Type domainType, TFileType fileType);

    CodeTypeReference GetCodeTypeReference(Type domainType, TFileType fileType);

    ICodeFileFactoryHeader? GetFileFactoryHeader(TFileType fileType, bool raiseIfNotFound = true);
}
