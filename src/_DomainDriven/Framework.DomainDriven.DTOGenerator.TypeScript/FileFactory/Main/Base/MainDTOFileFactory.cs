using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base
{
    /// <summary>
    /// MainDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public abstract class MainDTOFileFactory<TConfiguration> : BaseDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        protected MainDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService => new MainCodeTypeReferenceService<TConfiguration>(this.Configuration);

        protected sealed override bool? InternalBaseTypeContainsPropertyChange => true;

        protected override IEnumerable<string> GetImportedNamespaces()
        {
            return this.Configuration.GetNamespaces();
        }

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsClass = true,
                IsPartial = true,
                Attributes = MemberAttributes.Public
            };
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return CodeDomHelper.GenerateFromStaticInitializeMethodJs(this);

            yield return this.GenerateFromMethodsJs();

            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.StrictDTO))
            {
                yield return this.GenerateToStrictMethod();
            }

            if (this.Configuration.GeneratePolicy.Used(this.DomainType, this.FileType.AsObservableFileType()))
            {
                yield return this.GenerateFromObservableMethod();

                yield return this.GenerateToObservableMethod();
            }
        }

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            foreach (var customAttribute in base.GetCustomAttributes())
            {
                yield return customAttribute;
            }

            foreach (var knownTypesAttribute in this.Configuration.GetKnownTypesAttributes(this.FileType, this.DomainType))
            {
                yield return knownTypesAttribute;
            }
        }

        protected override IEnumerable<CodeAttributeDeclaration> GetPropertyCustomAttributes(PropertyInfo sourceProperty)
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            foreach (var customAttribute in base.GetPropertyCustomAttributes(sourceProperty))
            {
                yield return customAttribute;
            }

            foreach (var attributeDeclaration in sourceProperty.GetRestrictionCodeAttributeDeclarations())
            {
                yield return attributeDeclaration;
            }
        }

        protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
        {
            var defaultAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultAttribute != null)
            {
                return new CodePrimitiveExpression(defaultAttribute.Value);
            }

            if (property.PropertyType == typeof(bool))
            {
                return new CodePrimitiveExpression(false);
            }

            return null;
        }
    }
}
