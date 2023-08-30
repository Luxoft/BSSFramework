using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.Persistent;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class IntegrationSaveMethodGenerator<TConfiguration> : IntegrationBaseSaveMethodGenerator<TConfiguration>
        where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public IntegrationSaveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.IntegrationSave;

    protected override string Name => "Save" + this.DomainType.Name;

    protected override CodeTypeReference ReturnType => this.IdentTypeRef;


    protected override string GetComment()
    {
        return $"Save {this.DomainType.Name}";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.RichIntegrationTypeRef
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase());
    }


    protected override IEnumerable<CodeMemberMethod> GetFacadeMethods(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr)
    {
        foreach (var method in base.GetFacadeMethods(evaluateDataParameterExpr, bllParameterExpr))
        {
            yield return method;
        }

        yield return this.GetFacadeMethodWithBLL(evaluateDataParameterExpr, bllParameterExpr);
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        foreach (var baseStatement in base.GetFacadeMethodInternalStatements(evaluateDataExpr, bllRefExpr))
        {
            yield return baseStatement;
        }

        yield return new CodeThisReferenceExpression().ToMethodInvokeExpression(this.InternalName, this.Parameters.Select(decl => decl.ToVariableReferenceExpression()).Concat(new[] { evaluateDataExpr, bllRefExpr }))
                                                      .ToMethodReturnStatement();
    }


    public CodeMemberMethod GetFacadeMethodWithBLL(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr)
    {
        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Family,
                       Name = this.InternalName,
                       ReturnType = this.ReturnType,
               }.WithParameters(this.Parameters.Concat(new[] { evaluateDataParameterExpr, bllParameterExpr }))
                .WithStatements(this.GetFacadeMethodWithBLLStatements(evaluateDataParameterExpr.ToVariableReferenceExpression(), bllParameterExpr.ToVariableReferenceExpression()));
    }

    private IEnumerable<CodeStatement> GetFacadeMethodWithBLLStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));

        var savingObject = this.Parameter.ToVariableReferenceExpression();

        var allowCreate = this.Attribute.AllowCreate;

        var getByIdMethodParams = new List<CodeExpression>
                                  {
                                          savingObject.ToPropertyReference(this.Configuration.Environment.IdentityProperty),
                                          (!allowCreate).ToPrimitiveExpression()
                                  };

        if (this.DomainType.IsIntegrationVersion())
        {
            getByIdMethodParams.Add(new CodePrimitiveExpression(null));
            getByIdMethodParams.Add(LockRole.Update.ToPrimitiveExpression());
        }

        var saveObjectDecl = this.ToDomainObjectVarDecl(bllRefExpr.ToMethodInvokeExpression($"{nameof(IDefaultDomainBLLQueryBase<IIdentityObject<object>, IIdentityObject<object>, object>.GetById)}", getByIdMethodParams));

        yield return saveObjectDecl;

        var saveObjectRefExpr = saveObjectDecl.ToVariableReferenceExpression();

        if (allowCreate)
        {
            yield return new CodeNullConditionStatement(saveObjectRefExpr)
                         {
                                 TrueStatements =
                                 {
                                         this.DomainType.ToTypeReference().ToObjectCreateExpression().ToAssignStatement(saveObjectRefExpr)
                                 }
                         };
        }

        foreach (var codeStatement in this.ProcessIntegrationVersion(saveObjectRefExpr, savingObject, new[]{this.GetMappingMethod(evaluateDataExpr, savingObject, saveObjectDecl),this.GetInsertMethod(bllRefExpr, allowCreate, saveObjectDecl, savingObject)}))
        {
            yield return codeStatement;
        }

        yield return saveObjectDecl.ToVariableReferenceExpression()
                                   .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.IdentityDTO))
                                   .ToMethodReturnStatement();
    }

    private IEnumerable<CodeStatement> ProcessIntegrationVersion(
            CodeVariableReferenceExpression saveObjectRefExpr,
            CodeVariableReferenceExpression savingObject,
            CodeStatement[] logicCodeStatements)
    {
        var integrationVersionProperty = this.DomainType.TryGetIntegrationVersionProperty();

        if (null != integrationVersionProperty)
        {
            var domainObjectVersion = new CodePropertyReferenceExpression(saveObjectRefExpr, integrationVersionProperty.Name);

            var dtoVersion = new CodePropertyReferenceExpression(savingObject, integrationVersionProperty.Name);

            var codeBinaryOperatorType = integrationVersionProperty.ToCodeBinaryOperator();

            var less = new CodeBinaryOperatorExpression(
                                                        domainObjectVersion,
                                                        codeBinaryOperatorType,
                                                        dtoVersion);

            yield return new CodeConditionStatement(less, logicCodeStatements);
        }
        else
        {
            foreach (var logicCodeStatement in logicCodeStatements)
            {
                yield return logicCodeStatement;
            }
        }
    }

    private CodeStatement GetInsertMethod(CodeExpression bllRefExpr, bool allowCreate,
                                          CodeVariableDeclarationStatement saveObjectDecl, CodeVariableReferenceExpression savingObject) =>
            (allowCreate
                     ? bllRefExpr.ToMethodInvokeExpression(this.Configuration.InsertMethodName,
                                                           saveObjectDecl.ToVariableReferenceExpression(),
                                                           savingObject.ToPropertyReference(this.Configuration.Environment.IdentityProperty.Name))
                     : bllRefExpr.ToMethodInvokeExpression(this.Configuration.SaveMethodName,
                                                           saveObjectDecl.ToVariableReferenceExpression())).ToExpressionStatement();

    private CodeStatement GetMappingMethod(CodeExpression evaluateDataExpr, CodeVariableReferenceExpression savingObject,
                                           CodeVariableDeclarationStatement saveObjectDecl) =>
            savingObject.ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.MapToDomainObjectMethodName,
                                                  evaluateDataExpr.GetMappingService(), saveObjectDecl.ToVariableReferenceExpression())
                        .ToExpressionStatement();
}
