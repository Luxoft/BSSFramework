using System.CodeDom;

namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration.BLLFactoryContainer;

public interface IBLLFactoryContainerInterfaceGeneratorConfiguration
{
    CodeExpression GetCreateSecurityBLLExpr(CodeExpression logicExpressionSource, Type domainType, object securitySource);
}
