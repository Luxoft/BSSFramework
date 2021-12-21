using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

using Framework.CodeDom;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public class SimpleClientImplFileFactory<TConfiguration> : CodeFileFactory<TConfiguration, FileType>
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public SimpleClientImplFileFactory(TConfiguration configuration)
                : base(configuration, configuration.BaseContract)
        {
        }

        public override FileType FileType { get; } = FileType.SimpleClientImpl;

        public override CodeTypeReference BaseReference => typeof(ClientBase<>).ToTypeReference(this.Configuration.GetCodeTypeReference(this.DomainType, FileType.ClientContact));

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                Attributes = MemberAttributes.Public,
                IsPartial = true,
                IsClass = true
            };
        }

        protected override IEnumerable<CodeConstructor> GetConstructors()
        {
            yield return new CodeConstructor
            {
                    Attributes = MemberAttributes.Public
            };

            var bindingParameter = typeof(Binding).ToTypeReference().ToParameterDeclarationExpression("binding");
            var remoteAddressParameter = typeof(EndpointAddress).ToTypeReference().ToParameterDeclarationExpression("remoteAddress");

            yield return new CodeConstructor
            {
                    Attributes = MemberAttributes.Public,
                    Parameters = { bindingParameter, remoteAddressParameter },
                    BaseConstructorArgs = { bindingParameter.ToVariableReferenceExpression(), remoteAddressParameter.ToVariableReferenceExpression() }
            };
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return new CodeMemberProperty
                         {
                                 Name = "Channel",
                                 Attributes = MemberAttributes.New | MemberAttributes.Public,
                                 Type = this.Configuration.GetCodeTypeReference(this.DomainType, FileType.ClientContact),
                                 GetStatements = { new CodeBaseReferenceExpression().ToPropertyReference("Channel").ToMethodReturnStatement() }

            };
        }
    }
}
