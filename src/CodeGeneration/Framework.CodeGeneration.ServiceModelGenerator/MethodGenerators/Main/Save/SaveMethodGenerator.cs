using System.CodeDom;

using CommonFramework;

using Framework.BLL;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.Save.Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.Save;

public class SaveMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType) : BaseSaveMethodGenerator<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.Save;


    protected override string Name => "Save" + this.DomainType.Name;

    protected override CodeTypeReference ReturnType => this.SourceIdentTypeRef;

    protected override string GetComment() => $"Save {this.DomainType.GetPluralizedDomainName()}";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.StrictTypeRef.ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Strict");
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
        yield return new CodeThisReferenceExpression().ToMethodInvokeExpression(this.InternalName, this.Parameters.Select(decl => decl.ToVariableReferenceExpression()).Concat(
                                                                                    [evaluateDataExpr, bllRefExpr]))
                                                      .ToMethodReturnStatement();
    }


    public CodeMemberMethod GetFacadeMethodWithBLL(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr) =>
        new CodeMemberMethod { Attributes = MemberAttributes.Family, Name = this.InternalName, ReturnType = this.ReturnType, }
            .WithParameters([.. this.Parameters, evaluateDataParameterExpr, bllParameterExpr])
            .WithStatements(this.GetFacadeMethodWithBLLStatements(evaluateDataParameterExpr.ToVariableReferenceExpression(), bllParameterExpr.ToVariableReferenceExpression()));

    private IEnumerable<CodeStatement> GetFacadeMethodWithBLLStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var allowCreate = this.Attribute.AllowCreate;

        var saveObjectDecl = allowCreate ? this.ToDomainObjectVarDecl(bllRefExpr.ToStaticMethodInvokeExpression(typeof(DefaultDomainBLLBaseExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("GetByIdOrCreate"),
                                                                          this.Parameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty)))
                                     : this.ToDomainObjectVarDeclById(bllRefExpr);

        yield return saveObjectDecl;

        yield return this.Parameter
                         .ToVariableReferenceExpression()
                         .ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.MapToDomainObjectMethodName, evaluateDataExpr.GetMappingService(), saveObjectDecl.ToVariableReferenceExpression())
                         .ToExpressionStatement();

        yield return bllRefExpr.ToMethodInvokeExpression("Save", saveObjectDecl.ToVariableReferenceExpression())
                               .ToExpressionStatement();

        yield return saveObjectDecl.ToVariableReferenceExpression()
                                   .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.IdentityDTO))
                                   .ToMethodReturnStatement();
    }
}
