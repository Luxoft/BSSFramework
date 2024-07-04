using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class UpdateMethodGenerator<TConfiguration> : MethodGenerator<TConfiguration, BLLSaveRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public UpdateMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.Update;


    protected override string Name => "Update" + this.DomainType.Name;

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

    protected override SecurityRule SecurityRule { get; } = SecurityRule.Edit;


    protected override string GetComment()
    {
        return $"Update {this.DomainType.Name}";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration
                         .Environment.ServerDTO
                         .GetCodeTypeReference(this.DomainType, DTOType.UpdateDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Update");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var allowCreate = this.Attribute.AllowCreate;

        var saveObjectDecl = allowCreate ? this.ToDomainObjectVarDecl(bllRefExpr
                                                                              .ToStaticMethodInvokeExpression(typeof(DefaultDomainBLLBaseExtensions)
                                                                                          .ToTypeReferenceExpression()
                                                                                          .ToMethodReferenceExpression("GetByIdOrCreate"),
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

    //protected override BLLSaveRoleAttribute GetDefaultAttribute()
    //{
    //    return new BLLSaveRoleAttribute { AllowCreate = this.DomainType.HasDefaultConstructor() };
    //}
}
