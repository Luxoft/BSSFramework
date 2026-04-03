using System.CodeDom;

namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration;

public interface IBLLFactoryContainerInterfaceGeneratorConfiguration
{
    CodeExpression GetCreateSecurityBLLExpr(CodeExpression logicExpressionSource, Type domainType, object securitySource);
}
