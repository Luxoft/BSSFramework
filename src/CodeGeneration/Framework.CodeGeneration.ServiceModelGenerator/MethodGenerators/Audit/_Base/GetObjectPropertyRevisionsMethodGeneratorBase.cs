using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;
using Framework.Core;
using Framework.Infrastructure.Service;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Audit._Base;

public abstract class GetObjectPropertyRevisionsMethodGeneratorBase<TConfiguration>(
    TConfiguration configuration,
    Type domainType,
    IAuditDTOGeneratorConfigurationBase dtoConfiguration)
    : MethodGenerator<TConfiguration, BLLViewRoleAttribute>(configuration, domainType)
    where TConfiguration : class, IAuditGeneratorConfigurationBase<IAuditGenerationEnvironmentBase>
{
    protected override CodeTypeReference ReturnType => new(dtoConfiguration.DomainObjectPropertiesRevisionDTOFullTypeName);

    protected override bool IsEdit { get; } = false;


    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");

        yield return this.PropertyNameParameter;
    }

    private CodeParameterDeclarationExpression PropertyNameParameter => new(typeof(string), "propertyName");

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var auditServiceCodeReference = new CodeTypeReference(typeof(AuditService<,,,,,,>))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.GetIdentityType()))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.BLLCore.BLLFactoryInterfaceTypeReference))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.BLLCore.ActualRootSecurityServiceInterfaceType))

                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.PersistentDomainObjectBaseType))
                                        .Self(z => z.TypeArguments.Add(dtoConfiguration.DomainObjectPropertiesRevisionDTOFullTypeName))
                                        .Self(z => z.TypeArguments.Add(dtoConfiguration.PropertyRevisionFullTypeName));

        yield return auditServiceCodeReference
                     .ToObjectCreateExpression(evaluateDataExpr.GetContext())
                     .ToMethodInvokeExpression("GetPropertyChanges", this.GetBLLMethodParameters().ToArray())
                     .Self(z => z.Method.TypeArguments.Add(this.DomainType))
                     .ToMethodReturnStatement();
    }

    protected virtual IEnumerable<CodeExpression> GetBLLMethodParameters()
    {
        yield return this.Parameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty);
        yield return this.PropertyNameParameter.ToVariableReferenceExpression();
    }
}
