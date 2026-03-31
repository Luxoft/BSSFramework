using System.CodeDom;
using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;

public interface IPropertyAssigner : IDTOSource
{
    CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef);
}


public interface IPropertyAssigner<out TConfiguration> : IDTOSource<TConfiguration>, IPropertyAssigner
        where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>;
