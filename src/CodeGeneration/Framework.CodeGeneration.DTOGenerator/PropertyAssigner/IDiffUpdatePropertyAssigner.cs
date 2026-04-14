using System.CodeDom;
using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner;

public interface IDiffUpdatePropertyAssigner : IDTOSource
{
    CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression baseSourcePropertyRef, CodeExpression currentSourcePropertyRef, CodeExpression targetPropertyRef);
}
