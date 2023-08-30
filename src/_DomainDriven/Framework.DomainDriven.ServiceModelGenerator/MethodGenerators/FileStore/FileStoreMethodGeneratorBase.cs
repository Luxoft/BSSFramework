using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModelGenerator.MethodGenerators.FileStore;

public abstract class FileStoreMethodGeneratorBase<TConfiguration> : MethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IFileStoreGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected FileStoreMethodGeneratorBase(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    protected override bool IsEdit { get; } = false;


    protected override object GetBLLSecurityParameter()
    {
        return this.Configuration.TryGetSecurityAttribute(this.DomainType, this.IsEdit) ?? base.GetBLLSecurityParameter();
    }



    protected IEnumerable<CodeStatement> GetCheckAccessMethods(
            CodeExpression evaluateDataExpr,
            Type targetDomainType,
            CodeVariableReferenceExpression domainIdentRefExpression)
    {
        if (null != this.Configuration.Environment.BLLCore.GetBLLSecurityModeType(targetDomainType))
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
            attr =
                    this.Configuration.Environment.BLLCore.GetBLLSecurityModeType(targetDomainType)
                        .GetField("View")
                        .GetValue(null) as Enum;
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
