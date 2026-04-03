using System.CodeDom;
using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;

public interface IDiffUpdatePropertyAssigner : IDTOSource
{
    CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression baseSourcePropertyRef, CodeExpression currentSourcePropertyRef, CodeExpression targetPropertyRef);
}

public interface IiDiffUpdatePropertyAssigner<out TConfiguration> : IDTOSource<TConfiguration>, IDiffUpdatePropertyAssigner
        where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>;
