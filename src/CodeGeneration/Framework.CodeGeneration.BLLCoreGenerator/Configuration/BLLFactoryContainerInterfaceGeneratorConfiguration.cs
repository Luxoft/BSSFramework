using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.FileGeneration.Configuration;
using Anch.SecuritySystem;

namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration;

public class BLLFactoryContainerInterfaceGeneratorConfiguration<TConfiguration>(TConfiguration configuration)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), IBLLFactoryContainerInterfaceGeneratorConfiguration
    where TConfiguration : class, IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment>
{
    public CodeExpression GetCreateSecurityBLLExpr(CodeExpression logicExpressionSource, Type domainType, object? securitySource)
    {
        if (logicExpressionSource == null) throw new ArgumentNullException(nameof(logicExpressionSource));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (securitySource == null)
        {
            return logicExpressionSource.ToPropertyReference(domainType.Name);
        }
        else if (securitySource is SecurityRule securityRule)
        {
            return logicExpressionSource.ToPropertyReference(domainType.Name + "Factory")
                                        .ToMethodInvokeExpression("Create", this.Configuration.GetSecurityCodeExpression(securityRule));
        }
        else if (securitySource is CodeExpression arg)
        {
            return logicExpressionSource.ToPropertyReference(domainType.Name + "Factory")
                                        .ToMethodInvokeExpression("Create", arg);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
