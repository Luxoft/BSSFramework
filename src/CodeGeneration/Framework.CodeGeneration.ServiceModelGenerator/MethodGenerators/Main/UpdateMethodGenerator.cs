using System.CodeDom;

using Anch.Core;

using Framework.BLL;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main;

public class UpdateMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType) : MethodGenerator<TConfiguration, BLLSaveRoleAttribute>(configuration, domainType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.Update;


    protected override string Name => "Update" + this.DomainType.Name;

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

    protected override bool IsEdit { get; } = true;


    protected override string GetComment() => $"Update {this.DomainType.Name}";

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
