using System;
using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Visual
{
    /// <summary>
    /// Default observable visualDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultObservableVisualDTOFileFactory<TConfiguration> : ObservableFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultObservableVisualDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public override MainDTOFileType FileType => ObservableFileType.ObservableVisualDTO;

        public override CodeTypeReference BaseReference => this.Configuration.GetCodeTypeReference(this.DomainType, ObservableFileType.BaseObservablePersistentDTO);

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsClass = true,
                IsPartial = true,
                Attributes = MemberAttributes.Public
            };
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return this.GenerateDefaultConstructor();

            yield return this.GenerateVisualObservableFromPlainJs();
            yield return this.GenerateStaticFromPlainJsMethod();
            yield return this.GenerateToJsonMethod();

            if (this.IsPersistent())
            {
                if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
                {
                    yield return this.GetIdentityObjectContainerImplementation();
                }
            }
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }
        }

        public CodeMemberMethod GetIdentityObjectContainerImplementation()
        {
            var identityRef = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.IdentityDTO);
            var identityObjectContainerImplementation = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = "Identity",
                ReturnType = identityRef
            };

            identityObjectContainerImplementation.Statements.Add(
                identityRef.ToObjectCreateExpression(new CodeThisReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.IdentityProperty.Name))
                    .ToMethodReturnStatement());

            return identityObjectContainerImplementation;
        }
    }
}
