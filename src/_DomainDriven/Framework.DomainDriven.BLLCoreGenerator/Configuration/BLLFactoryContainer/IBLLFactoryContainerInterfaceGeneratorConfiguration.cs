using System.CodeDom;

namespace Framework.DomainDriven.BLLCoreGenerator;

public interface IBLLFactoryContainerInterfaceGeneratorConfiguration
{
    CodeExpression GetCreateSecurityBLLExpr(CodeExpression logicExpressionSource, Type domainType, object securitySource);
}
