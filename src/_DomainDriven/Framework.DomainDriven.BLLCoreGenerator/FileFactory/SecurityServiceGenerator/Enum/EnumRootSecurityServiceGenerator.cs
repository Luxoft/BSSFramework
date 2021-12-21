using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class EnumRootSecurityServiceGenerator<TConfiguration> : RootSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public EnumRootSecurityServiceGenerator(TConfiguration configuration)
            : base(configuration)
        {
        }

        protected override IDomainSecurityServiceGenerator GetDomainSecurityServiceGeneratorInternal(Type domainType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            var dependencySecurityAttr = domainType.GetCustomAttribute<DependencySecurityAttribute>();

            if (dependencySecurityAttr == null)
            {
                if (domainType.HasAttribute<CustomContextSecurityAttribute>())
                {
                    return new CustomContextDomainSecurityServiceGenerator<TConfiguration>(this.Configuration, domainType);
                }
                else
                {
                    return new EnumDomainSecurityServiceGenerator<TConfiguration>(this.Configuration, domainType);
                }
            }
            else
            {
                if (dependencySecurityAttr.IsUntyped)
                {
                    return new UntypedDependencyDomainSecurityServiceGenerator<TConfiguration>(this.Configuration, domainType, dependencySecurityAttr);
                }
                else
                {
                    return new DependencyDomainSecurityServiceGenerator<TConfiguration>(this.Configuration, domainType, dependencySecurityAttr);
                }
            }
        }

        public override IEnumerable<CodeTypeMember> GetBaseMembers()
        {
            return from domainType in this.Configuration.SecurityServiceDomainTypes

                   where !domainType.IsProjection()

                   where this.Configuration.HasSecurityContext(domainType)

                   where !domainType.HasAttribute<CustomContextSecurityAttribute>() && !domainType.HasAttribute<DependencySecurityAttribute>()

                   let typeParameters = this.Configuration.GetDomainTypeSecurityParameters(domainType).ToArray()

                   let domainTypeRef = typeParameters.Select(p => p.ToTypeReference()).FirstOr(() => domainType.ToTypeReference())

                   select new CodeMemberMethod
                   {
                       Name = domainType.ToGetSecurityPathMethodName(),
                       Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
                       ReturnType = this.Configuration.GetCodeTypeReference(null, FileType.SecurityPath).ToTypeReference(domainTypeRef),
                   }.WithTypeParameters(typeParameters);
        }

        protected override bool IsSecurityServiceDomainType(Type domainType)
        {
            var enumOperationsRequest = from securityOperationCode in this.Configuration.Environment.GetSecurityOperationCodes()

                                        let fieldInfo = securityOperationCode.ToFieldInfo()

                                        let securityOperationAttribute = securityOperationCode.GetSecurityOperationAttribute()

                                        where securityOperationAttribute != null
                                           && !securityOperationAttribute.IsClient
                                           && domainType.Name.Equals(securityOperationAttribute.DomainType, StringComparison.CurrentCultureIgnoreCase)

                                        select securityOperationCode;

            return base.IsSecurityServiceDomainType(domainType) || enumOperationsRequest.Any();
        }

        public override IEnumerable<CodeTypeReference> GetBLLContextBaseTypes()
        {
            yield return typeof(ISecurityOperationResolver<,>).MakeGenericType(this.Configuration.Environment.PersistentDomainObjectBaseType, this.Configuration.Environment.SecurityOperationCodeType).ToTypeReference();
        }

        public override IEnumerable<CodeTypeMember> GetBLLContextMembers()
        {
            var securityOperationType = typeof(SecurityOperation<>).MakeGenericType(this.Configuration.Environment.SecurityOperationCodeType).ToTypeReference();

            {
                var codeParameter = this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference().ToParameterDeclarationExpression("securityOperationCode");
                var codeParameterRefExp = codeParameter.ToVariableReferenceExpression();

                yield return new CodeMemberMethod
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = securityOperationType,
                    Name = "GetSecurityOperation",
                    Parameters = { codeParameter },
                    Statements =
                        {
                            this.Configuration.SecurityOperationTypeReference
                                              .ToTypeReferenceExpression()
                                              .ToMethodInvokeExpression(this.Configuration.GetOperationByCodeMethodName, codeParameterRefExp)
                                              .ToMethodReturnStatement()
                        }
                };
            }

            {
                var modeParameter = typeof(BLLSecurityMode).ToTypeReference().ToParameterDeclarationExpression("securitMode");
                var modeParameterRefExp = modeParameter.ToVariableReferenceExpression();

                var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter(false);
                var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

                yield return new CodeMemberMethod
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Override,
                    ReturnType = securityOperationType,
                    Name = "GetSecurityOperation",
                    Parameters = { modeParameter },
                    Statements =
                        {
                            this.Configuration.SecurityOperationTypeReference
                                              .ToTypeReferenceExpression()
                                              .ToMethodReferenceExpression(this.Configuration.GetOperationByModeMethodName, genericDomainObjectParameterTypeRef)
                                              .ToMethodInvokeExpression(modeParameterRefExp)
                                              .ToMethodReturnStatement()
                        },
                    TypeParameters = { genericDomainObjectParameter }
                };
            }
        }

        public override CodeTypeReference GetGenericRootSecurityServiceType()
        {
            return typeof(RootSecurityService<,,>).ToTypeReference(this.Configuration.BLLContextInterfaceTypeReference, this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference());
        }

        public override CodeTypeReference GetGenericRootSecurityServiceInterfaceType()
        {
            return typeof(IRootSecurityService<,,>).ToTypeReference(this.Configuration.BLLContextInterfaceTypeReference, this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference());
        }

        public override CodeTypeReference GetDomainInterfaceBaseServiceType()
        {
            var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter(false);
            var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

            return typeof(IDomainSecurityService<,>)
                .ToTypeReference(genericDomainObjectParameterTypeRef,
                                 this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference());
        }
    }
}
