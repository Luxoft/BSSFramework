using System.CodeDom;

namespace Framework.CodeGeneration.DTOGenerator.FileFactory.ClientMapping;

public interface IClientMappingServiceExternalMethodGenerator
{
    IEnumerable<CodeMemberMethod> GetClientMappingServiceMethods();

    IEnumerable<CodeMemberMethod> GetClientMappingServiceInterfaceMethods();
}
