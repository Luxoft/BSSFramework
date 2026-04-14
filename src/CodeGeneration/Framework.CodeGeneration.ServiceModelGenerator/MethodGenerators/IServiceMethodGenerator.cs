using System.CodeDom;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators;

public interface IServiceMethodGenerator
{
    MethodIdentity Identity { get; }

    CodeMemberMethod GetContractMethod();

    IEnumerable<CodeMemberMethod> GetFacadeMethods();
}
