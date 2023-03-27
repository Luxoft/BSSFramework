using System.CodeDom;

namespace Framework.DomainDriven.DTOGenerator;

public interface IClientMappingServiceExternalMethodGenerator
{
    IEnumerable<CodeMemberMethod> GetClientMappingServiceMethods();

    IEnumerable<CodeMemberMethod> GetClientMappingServiceInterfaceMethods();
}
