using System.CodeDom;
using System.Collections.Generic;

namespace Framework.DomainDriven.DTOGenerator
{
    public interface IClientMappingServiceExternalMethodGenerator
    {
        IEnumerable<CodeMemberMethod> GetClientMappingServiceMethods();

        IEnumerable<CodeMemberMethod> GetClientMappingServiceInterfaceMethods();
    }
}
