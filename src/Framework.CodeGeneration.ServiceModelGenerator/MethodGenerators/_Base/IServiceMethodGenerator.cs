using System.CodeDom;

namespace Framework.DomainDriven.ServiceModelGenerator;

public interface IServiceMethodGenerator
{
    MethodIdentity Identity { get; }

    CodeMemberMethod GetContractMethod();

    IEnumerable<CodeMemberMethod> GetFacadeMethods();
}
