using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class EnumDomainSecurityServiceGenerator<TConfiguration> : DomainSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        private readonly CodeMemberMethod getSecurityPathMethod;

        private readonly CodeMemberField securityPathContainerField;

        private readonly bool hasContext;


        public EnumDomainSecurityServiceGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            var genericTypes = this.Configuration.GetDomainTypeSecurityParameters(this.DomainType).Select(p => p.ToTypeReference()).ToArray();

            this.DomainTypeReference = genericTypes.FirstOr(() => this.DomainType.ToTypeReference());


            this.hasContext = this.Configuration.HasSecurityContext(this.DomainType);

            this.BaseServiceType = (this.hasContext ? typeof(ContextDomainSecurityService<,,,>) : typeof(NonContextDomainSecurityService<,,,>)).ToTypeReference(

                this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                this.DomainTypeReference,
                this.Configuration.Environment.GetIdentityType().ToTypeReference(),
                this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference());


            if (this.hasContext)
            {
                this.securityPathContainerField = this.Configuration.GetCodeTypeReference(null, FileType.RootSecurityServicePathContainerInterface).ToMemberField("securityPathContainer");

                this.getSecurityPathMethod = new CodeMemberMethod
                {
                    Name = "GetSecurityPath",
                    Attributes = MemberAttributes.Family | MemberAttributes.Override,
                    ReturnType = typeof(SecurityPathBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), this.DomainTypeReference, this.Configuration.Environment.GetIdentityType().ToTypeReference()),
                    Statements =
                    {
                        new CodeThisReferenceExpression().ToFieldReference(this.securityPathContainerField).ToMethodReferenceExpression(domainType.ToGetSecurityPathMethodName(), genericTypes).ToMethodInvokeExpression().ToMethodReturnStatement()
                    }
                };
            }
        }


        public sealed override CodeTypeReference DomainTypeReference { get; }


        public override CodeTypeReference BaseServiceType { get; }


        public override IEnumerable<CodeTypeMember> GetMembers()
        {
            if (this.securityPathContainerField != null)
            {
                yield return this.securityPathContainerField;
            }

            if (this.getSecurityPathMethod != null)
            {
                yield return this.getSecurityPathMethod;
            }
        }

        public override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            yield break;
        }

        public override IEnumerable<(CodeTypeReference ParameterType, string Name)> GetBaseTypeConstructorParameters()
        {
            yield return (typeof(IDisabledSecurityProviderContainer<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType), "disabledSecurityProviderContainer");
            yield return (typeof(ISecurityOperationResolver<,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType, this.Configuration.Environment.SecurityOperationCodeType), "securityOperationResolver");
            yield return (typeof(IAuthorizationSystem<>).ToTypeReference(this.Configuration.Environment.GetIdentityType()), "authorizationSystem");

            if (this.hasContext)
            {
                yield return (typeof(ISecurityExpressionBuilderFactory<,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType, this.Configuration.Environment.GetIdentityType()), "securityExpressionBuilderFactory");
            }
        }

        public override CodeConstructor GetConstructor()
        {
            var ctor = base.GetConstructor();

            if (this.securityPathContainerField != null)
            {
                var initFieldParameter = this.securityPathContainerField.Type.ToParameterDeclarationExpression(this.securityPathContainerField.Name);

                ctor.Parameters.Add(initFieldParameter);
                ctor.Statements.Add(initFieldParameter.ToVariableReferenceExpression().ToAssignStatement(new CodeThisReferenceExpression().ToFieldReference(this.securityPathContainerField)));
            }

            return ctor;
        }
    }
}
