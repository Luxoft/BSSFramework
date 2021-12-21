using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory
{
    /// <summary>
    /// Фабрика ObservableIdentityDTO.
    /// </summary>
    /// <typeparam name="TConfiguration">Тип конфигурации.</typeparam>
    /// <seealso cref="Main.Base.ObservableDTOFileFactory{TConfiguration}" />
    public class DefaultObservableIdentityDTOFileFactory<TConfiguration> : ObservableDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        /// <summary>
        /// Создаёт экземпляр класса <see cref="DefaultObservableIdentityDTOFileFactory{TConfiguration}"/>.
        /// </summary>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="domainType">Тип доменного объекта.</param>
        public DefaultObservableIdentityDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        /// <inheritdoc />
        public override MainDTOFileType FileType { get; } = ObservableFileType.ObservableIdentityDTO;

        /// <inheritdoc />
        public override CodeTypeReference BaseReference => null;

        /// <inheritdoc />
        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsClass = this.Configuration.IdentityIsReference,
                IsStruct = !this.Configuration.IdentityIsReference,
                Attributes = MemberAttributes.Public
            };
        }

        /// <inheritdoc />
        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            yield return this.GetIdCodeMemberField();

            yield return this.GenerateIdentityConstructor();

            yield return this.GenerateIdentityFromStaticInitializeMethodJs();
        }

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            yield return this.GetDataContractCodeAttributeDeclaration();
        }

        private string IdPropertyName => this.Configuration.Environment.IdentityProperty.Name;

        private CodeMemberField GetIdCodeMemberField()
        {
            return new CodeMemberField(Constants.IdentityTypeName, this.IdPropertyName)
            {
                Attributes = MemberAttributes.Public,
                CustomAttributes = { new CodeAttributeDeclaration(new CodeTypeReference(typeof(DataMemberAttribute))) }
            };
        }

        private CodeConstructor GenerateIdentityConstructor()
        {
            var sourceParameter = new CodeParameterDeclarationExpression(Constants.IdentityTypeName, "id");

            var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

            return new CodeConstructor
            {
                Parameters = { sourceParameter },
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                Statements =
                       {
                           new CodeThrowArgumentIsNullOrUndefinedExceptionConditionStatement(sourceParameter),
                           sourceParameterRef
                               .ToAssignStatement(
                                   new CodeThisReferenceExpression()
                                       .ToPropertyReference(this.IdPropertyName))
                       }
            };
        }
    }
}
