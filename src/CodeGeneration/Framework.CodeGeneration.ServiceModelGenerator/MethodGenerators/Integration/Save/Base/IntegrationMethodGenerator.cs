using System.CodeDom;

using Framework.BLL;
using Framework.BLL.Domain.ServiceRole.Base;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Integration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;

using SecuritySystem;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Save.Base;

public abstract class IntegrationMethodGenerator<TConfiguration, TBLLRoleAttribute>(TConfiguration configuration, Type domainType)
    : MethodGenerator<TConfiguration, TBLLRoleAttribute>(configuration, domainType)
    where TConfiguration : class, IIntegrationGeneratorConfiguration<IServiceModelGenerationEnvironment>
    where TBLLRoleAttribute : BLLServiceRoleAttribute
{
    protected sealed override bool RequiredSecurity { get; } = false;

    protected override bool IsEdit { get; } = true;


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        yield return evaluateDataExpr.GetContext()
                                     .ToPropertyReference(nameof(IAuthorizationBLLContextContainer<>.Authorization))
                                     .ToPropertyReference(nameof(SecuritySystem))
                                     .ToMethodInvokeExpression(nameof(ISecuritySystem.CheckAccessAsync), this.Configuration.IntegrationSecurityRule)
                                     .ToExpressionStatement();
    }
}
