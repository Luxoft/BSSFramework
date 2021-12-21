using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class FixedDomainSecurityServiceGenerator<TConfiguration> : DomainSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public FixedDomainSecurityServiceGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.BaseServiceType = typeof(DomainSecurityService<>).ToTypeReference(this.DomainType);
        }


        public override CodeTypeReference BaseServiceType { get; }


        public override IEnumerable<CodeTypeMember> GetMembers()
        {
            var securityServiceBaseParameterFieldRefExpr = new CodeThisReferenceExpression().ToFieldReference("_serviceBase");

            var securityModeParameter = this.Configuration.GetBLLSecurityModeType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("securityMode");

            yield return new CodeMemberMethod
            {
                Name = "CreateSecurityProvider",
                Attributes = MemberAttributes.Family | MemberAttributes.Override,
                ReturnType = typeof(ISecurityProvider<>).ToTypeReference(this.DomainType.ToTypeReference()),
                Parameters = { securityModeParameter },
                Statements = { securityServiceBaseParameterFieldRefExpr
                    .ToMethodInvokeExpression($"Get{this.DomainType.Name}SecurityProvider", securityModeParameter.ToVariableReferenceExpression())
                    .ToMethodReturnStatement() }
            };
        }

        public override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            yield break;
        }

        public override IEnumerable<(CodeTypeReference ParameterType, string Name)> GetBaseTypeConstructorParameters()
        {
            yield return (typeof(IAccessDeniedExceptionService<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType), "accessDeniedExceptionService");
        }
    }
}
