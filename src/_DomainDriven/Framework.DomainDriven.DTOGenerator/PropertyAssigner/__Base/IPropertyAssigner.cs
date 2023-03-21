using System.CodeDom;
using System.Reflection;

namespace Framework.DomainDriven.DTOGenerator;

public interface IPropertyAssigner : IDTOSource
{
    CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef);
}


public interface IPropertyAssigner<out TConfiguration> : IDTOSource<TConfiguration>, IPropertyAssigner
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
}
