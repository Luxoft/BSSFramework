using System.CodeDom;

namespace Framework.DomainDriven.Generation;

public interface IMethodGenerator
{
    CodeMemberMethod GetMethod();
}
