using System.CodeDom;

using Framework.BLL;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStore;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;

using SecuritySystem;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.FileStore;

public abstract class FileStoreMethodGeneratorBase<TConfiguration>(TConfiguration configuration, Type domainType)
    : MethodGenerator<TConfiguration, BLLViewRoleAttribute>(configuration, domainType)
    where TConfiguration : class, IFileStoreGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    protected override bool IsEdit { get; } = false;


    protected override object? GetBLLSecurityParameter(CodeExpression evaluateDataExpr) => this.Configuration.TryGetSecurityAttribute(this.DomainType, this.IsEdit) ?? base.GetBLLSecurityParameter(evaluateDataExpr);

    protected IEnumerable<CodeStatement> GetCheckAccessMethods(
            CodeExpression evaluateDataExpr,
            Type targetDomainType,
            CodeVariableReferenceExpression domainIdentRefExpression)
    {

        var targetBLLDecl = this.GetTargetBLLVariableDeclaration(evaluateDataExpr, targetDomainType);

        yield return targetBLLDecl;

        var bllReference = targetDomainType.ToTypeReference();

        var bllExpression = targetBLLDecl.ToVariableReferenceExpression();

        var byIdExprByIdentityRef = this.Configuration.GetByIdExprByIdentityRef(bllExpression, domainIdentRefExpression);

        var targetDomainDecl = bllReference.ToVariableDeclarationStatement("targetDomainObject", byIdExprByIdentityRef);

        yield return targetDomainDecl;


        yield return bllExpression
                     .ToMethodInvokeExpression("CheckAccess", targetDomainDecl.ToVariableReferenceExpression())
                     .ToExpressionStatement();
    }

    private CodeVariableDeclarationStatement GetTargetBLLVariableDeclaration(CodeExpression evaluateDataExpr, Type targetDomainType)
    {
        if (evaluateDataExpr == null)
        {
            throw new ArgumentNullException(nameof(evaluateDataExpr));
        }

        var attr = this.Configuration.TryGetSecurityAttribute(targetDomainType, false);

        if (null == attr)
        {
            attr = SecurityRule.View;
        }

        var bllRef = this.Configuration.Environment.BLLCore.GetCodeTypeReference(
                                                                                 targetDomainType,
                                                                                 BLLCoreGenerator.FileType.BLLInterface);

        var bllCreateExpr =
                this.Configuration.Environment.BLLCore.Logics.GetCreateSecurityBLLExpr(
                                                                                       evaluateDataExpr.GetContext()
                                                                                               .ToPropertyReference((IBLLFactoryContainerContext<object> context) => context.Logics),
                                                                                       targetDomainType,
                                                                                       attr);

        return bllRef.ToVariableDeclarationStatement("targetBLL", bllCreateExpr);
    }
}
