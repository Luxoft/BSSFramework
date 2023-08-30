using System.CodeDom;

using Framework.DomainDriven.Generation;

namespace Framework.DomainDriven.BLLCoreGenerator;

public interface IBLLFactoryContainerInterfaceGeneratorConfiguration
{
    IEnumerable<ICodeFile> GetFileFactories();

    CodeExpression GetCreateSecurityBLLExpr(CodeExpression logicExpressionSource, Type domainType, object securitySource);
}
