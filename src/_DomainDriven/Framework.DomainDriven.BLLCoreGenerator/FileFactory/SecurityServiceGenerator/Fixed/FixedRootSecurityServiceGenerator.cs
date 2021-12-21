using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class FixedRootSecurityServiceGenerator<TConfiguration> : RootSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public FixedRootSecurityServiceGenerator(TConfiguration configuration)
            : base(configuration)
        {

        }


        protected override IDomainSecurityServiceGenerator GetDomainSecurityServiceGeneratorInternal(Type domainType)
        {
            return new FixedDomainSecurityServiceGenerator<TConfiguration>(this.Configuration, domainType);
        }


        public override IEnumerable<CodeTypeMember> GetBaseMembers()
        {
            return from domainType in this.Configuration.SecurityServiceDomainTypes

                   let securityModeParameter = this.Configuration.GetBLLSecurityModeType(domainType).ToTypeReference().ToParameterDeclarationExpression("securityMode")

                   select new CodeMemberMethod
                   {
                       Name = domainType.ToGetSecurityProviderMethodName(),
                       Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                       ReturnType = typeof(ISecurityProvider<>).ToTypeReference(domainType.ToTypeReference()),
                       Parameters = { securityModeParameter }
                   };
        }


        public override IEnumerable<CodeTypeReference> GetBLLContextBaseTypes()
        {
            yield break;
        }

        public override IEnumerable<CodeTypeMember> GetBLLContextMembers()
        {
            yield break;
        }

        public override CodeTypeReference GetGenericRootSecurityServiceType()
        {
            return typeof(RootSecurityService<,,>)
                  .ToTypeReference(this.Configuration.BLLContextInterfaceTypeReference, this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference());
        }

        public override CodeTypeReference GetGenericRootSecurityServiceInterfaceType()
        {
            return typeof(IRootSecurityService<,>).ToTypeReference(this.Configuration.BLLContextInterfaceTypeReference, this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference());
        }

        public override CodeTypeReference GetDomainInterfaceBaseServiceType()
        {
            var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter(false);
            var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

            return typeof(IDomainSecurityService<>)
                .ToTypeReference(genericDomainObjectParameterTypeRef);
        }
    }
}
