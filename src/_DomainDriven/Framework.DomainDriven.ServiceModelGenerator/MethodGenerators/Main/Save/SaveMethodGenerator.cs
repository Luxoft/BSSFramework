using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class SaveMethodGenerator<TConfiguration> : BaseSaveMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public SaveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.Save;


    protected override string Name => "Save" + this.DomainType.Name;

    protected override CodeTypeReference ReturnType => this.SourceIdentTypeRef;

    protected override string GetComment()
    {
        return $"Save {this.DomainType.GetPluralizedDomainName()}";
    }

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
