using System.CodeDom;
using System.Collections.Generic;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public interface IServerMappingServiceExternalMethodGenerator
    {
        IEnumerable<CodeMemberMethod> GetServerMappingServiceMethods();

        IEnumerable<CodeMemberMethod> GetServerMappingServiceInterfaceMethods();
    }
}
