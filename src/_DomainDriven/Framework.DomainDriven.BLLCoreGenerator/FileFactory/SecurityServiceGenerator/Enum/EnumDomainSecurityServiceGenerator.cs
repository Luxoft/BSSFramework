﻿using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.Services;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class EnumDomainSecurityServiceGenerator<TConfiguration> : DomainSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public EnumDomainSecurityServiceGenerator(TConfiguration configuration, Type domainType)
        : base(configuration, domainType)
    {
        var genericTypes = this.Configuration.GetDomainTypeSecurityParameters(this.DomainType).Select(p => p.ToTypeReference()).ToArray();

        this.DomainTypeReference = genericTypes.FirstOr(() => this.DomainType.ToTypeReference());

        this.BaseServiceType = typeof(ContextDomainSecurityService<>).ToTypeReference(this.DomainTypeReference);
    }


    public sealed override CodeTypeReference DomainTypeReference { get; }


    public override CodeTypeReference BaseServiceType { get; }


    public override IEnumerable<CodeTypeMember> GetMembers()
    {
        yield break;
    }

    public override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield break;
    }

    public override IEnumerable<(CodeTypeReference ParameterType, string Name, CodeExpression CustomBaseInvoke)> GetBaseTypeConstructorParameters()
    {
        yield return (typeof(ISecurityRuleExpander).ToTypeReference(), "securityRuleExpander", null);

        {
            yield return (typeof(IDomainSecurityProviderFactory<>).ToTypeReference(this.DomainTypeReference), "domainSecurityProviderFactory", null);

            var securityPathContainerParam = new CodeParameterDeclarationExpression(this.Configuration.GetCodeTypeReference(null, FileType.RootSecurityServicePathContainerInterface), "securityPathContainer");

            yield return (
                             securityPathContainerParam.Type,
                             securityPathContainerParam.Name,
                             securityPathContainerParam.ToVariableReferenceExpression().ToMethodInvokeExpression(this.DomainType.ToGetSecurityPathMethodName())
                         );
        }
    }
}
