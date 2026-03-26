using System.CodeDom;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Mapping;

public interface IServerMappingServiceExternalMethodGenerator
{
    IEnumerable<CodeMemberMethod> GetServerMappingServiceMethods();

    IEnumerable<CodeMemberMethod> GetServerMappingServiceInterfaceMethods();
}
