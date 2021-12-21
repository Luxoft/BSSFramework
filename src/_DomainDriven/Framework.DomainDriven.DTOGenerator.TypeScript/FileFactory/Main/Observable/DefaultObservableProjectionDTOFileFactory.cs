using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator.TypeScript.CodeTypeReferenceService;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;
using Framework.Projection;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable
{
    /// <summary>
    /// Default base observable projection DTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultObservableProjectionDTOFileFactory<TConfiguration> : ObservableFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        private readonly Type sourceType;

        public DefaultObservableProjectionDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.CodeTypeReferenceService = new ProjectionObservableCodeTypeReferenceService<TConfiguration>(this.Configuration);

            this.sourceType = this.DomainType.GetProjectionSourceType();
        }

        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }

        public override MainDTOFileType FileType => ObservableFileType.ObservableProjectionDTO;

        public override CodeTypeReference BaseReference =>

        this.Configuration.GetCodeTypeReference(
        this.DomainType,
        this.IsPersistent() ? ObservableFileType.BaseObservablePersistentDTO : ObservableFileType.BaseObservableAbstractDTO);

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return this.GenerateDefaultConstructor();
            yield return this.GenerateToJsonMethod();

            yield return this.GenerateObservableProjectionFromPlainJs(false);
            yield return this.GenerateStaticFromPlainJsMethod();

            if (this.IsPersistent())
            {
                if (this.Configuration.GeneratePolicy.Used(this.sourceType, DTOGenerator.FileType.IdentityDTO))
                {
                    var identityRef = this.Configuration.GetCodeTypeReference(this.DomainType.GetProjectionSourceTypeOrSelf(), DTOType.IdentityDTO);

                    yield return this.GetIdentityObjectContainerImplementation(identityRef);
                }

                yield return new CodeMemberProperty
                {
                    Attributes = MemberAttributes.Public,
                    Name = "IsNew",
                    Type = new CodeTypeReference(typeof(bool)),
                    GetStatements =
                                 {
                                     new CodeMethodReturnStatement(
                                         new CodeBinaryOperatorExpression(
                                             new CodeDefaultValueExpression(new CodeTypeReference(this.Configuration.Environment.IdentityProperty.PropertyType)),
                                             CodeBinaryOperatorType.IdentityEquality,
                                             new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), this.Configuration.Environment.IdentityProperty.Name).UnwrapObservableProperty()))
                                 }
                };
            }
        }
    }
}
