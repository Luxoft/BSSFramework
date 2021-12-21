using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;
using Framework.DomainDriven.Generation.Domain;

using JetBrains.Annotations;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public class ClientContactFileFactory<TConfiguration> : CodeFileFactory<TConfiguration, FileType>
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public ClientContactFileFactory(TConfiguration configuration)
                : base(configuration, configuration.BaseContract)
        {
        }

        public override FileType FileType { get; } = FileType.ClientContact;

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                Attributes = MemberAttributes.Public,
                IsPartial = true,
                IsInterface = true
            };
        }

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            yield return this.Configuration.GetServiceContractAttribute();
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            return from sourceMethod in this.DomainType.ExtractContractMethods()

                   where this.Configuration.Policy.Used(sourceMethod)

                   from clientMethod in new[] { this.GetBeginMethod(sourceMethod), this.GetEndMethod(sourceMethod) }

                   select clientMethod;
        }

        protected virtual CodeMemberMethod GetBeginMethod([NotNull] MethodInfo sourceMethod)
        {
            if (sourceMethod == null) { throw new ArgumentNullException(nameof(sourceMethod)); }

            return new CodeMemberMethod
            {
                Name = $"Begin{sourceMethod.Name}",
                ReturnType = typeof(IAsyncResult).ToTypeReference(),
                CustomAttributes = { this.Configuration.GetOperationContractAttribute(sourceMethod) }
            }.WithParameters(sourceMethod.GetParameters().Select(parameter => this.Configuration.ResolveMethodParameterType(parameter.ParameterType).ToParameterDeclarationExpression(parameter.Name)))
                    .WithParameters(new[] { this.Configuration.ResolveMethodParameterType(typeof(AsyncCallback)).ToParameterDeclarationExpression("callback"),
                                            this.Configuration.ResolveMethodParameterType(typeof(object)).ToParameterDeclarationExpression("asyncState") });
        }

        protected virtual CodeMemberMethod GetEndMethod([NotNull] MethodInfo sourceMethod)
        {
            if (sourceMethod == null) { throw new ArgumentNullException(nameof(sourceMethod)); }

            return new CodeMemberMethod
            {
                Name = $"End{sourceMethod.Name}",
                ReturnType = this.Configuration.ResolveMethodParameterType(sourceMethod.ReturnType),
                Parameters = { this.Configuration.ResolveMethodParameterType(typeof(IAsyncResult)).ToParameterDeclarationExpression("result") }
            };
        }
    }
}
