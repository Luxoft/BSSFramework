using System.CodeDom;
using System.Reflection;

namespace Framework.DomainDriven.DTOGenerator;

public interface IDiffUpdatePropertyAssigner : IDTOSource
{
    CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression baseSourcePropertyRef, CodeExpression currentSourcePropertyRef, CodeExpression targetPropertyRef);
}

public interface IIDiffUpdatePropertyAssigner<out TConfiguration> : IDTOSource<TConfiguration>, IDiffUpdatePropertyAssigner
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
}
