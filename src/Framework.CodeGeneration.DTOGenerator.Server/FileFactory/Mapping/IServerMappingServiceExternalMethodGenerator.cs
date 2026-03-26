using System.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Server;

public interface IServerMappingServiceExternalMethodGenerator
{
    IEnumerable<CodeMemberMethod> GetServerMappingServiceMethods();

    IEnumerable<CodeMemberMethod> GetServerMappingServiceInterfaceMethods();
}
