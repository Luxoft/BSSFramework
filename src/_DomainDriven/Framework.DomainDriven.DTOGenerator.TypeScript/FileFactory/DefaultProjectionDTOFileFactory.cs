using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;
using Framework.Projection;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory
{
    public class DefaultProjectionDTOFileFactory<TConfiguration> : BaseDTOFileFactory<TConfiguration>
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        private readonly Type sourceType;

        public DefaultProjectionDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.CodeTypeReferenceService = new ProjectionCodeTypeReferenceService<TConfiguration>(this.Configuration);

            this.sourceType = this.DomainType.GetProjectionSourceType();
        }

        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }

        public override MainDTOFileType FileType { get; } = ClientFileType.ProjectionDTO;

        public override CodeTypeReference BaseReference =>

                this.IsPersistent() ? this.Configuration.GetBasePersistentReference() : this.Configuration.GetBaseAbstractReference();

        protected sealed override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.Public
            };
        }

        protected override bool? InternalBaseTypeContainsPropertyChange { get; }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return this.GenerateDefaultConstructor();

            yield return this.GenerateFromMethodsJs();

            if (this.Configuration.GeneratePolicy.Used(this.DomainType, this.FileType.AsObservableFileType()))
            {
                yield return this.GenerateFromObservableMethod();

                yield return this.GenerateToObservableMethod();
            }

            yield return CodeDomHelper.GenerateFromStaticInitializeMethodJs(this);

            if (this.IsPersistent())
            {
                if (this.Configuration.GeneratePolicy.Used(this.sourceType, DTOGenerator.FileType.IdentityDTO))
                {
                    yield return this.GetIdentityObjectContainerImplementation();
                }
            }
        }
    }
}
