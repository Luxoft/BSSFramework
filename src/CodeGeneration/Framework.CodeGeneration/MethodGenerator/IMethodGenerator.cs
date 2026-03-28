using System.CodeDom;

namespace Framework.CodeGeneration.MethodGenerator;

public interface IMethodGenerator
{
    CodeMemberMethod GetMethod();
}
