using System.CodeDom;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;

public interface IServiceMethodGenerator
{
    MethodIdentity Identity { get; }

    CodeMemberMethod GetContractMethod();

    IEnumerable<CodeMemberMethod> GetFacadeMethods();
}
