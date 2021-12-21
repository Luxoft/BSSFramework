using System.CodeDom;
using System.Collections.Generic;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public interface IServiceMethodGenerator
    {
        MethodIdentity Identity { get; }

        CodeMemberMethod GetContractMethod();

        IEnumerable<CodeMemberMethod> GetFacadeMethods();
    }
}