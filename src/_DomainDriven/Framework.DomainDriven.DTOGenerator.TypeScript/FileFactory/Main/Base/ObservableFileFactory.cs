using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.CodeTypeReferenceService;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

/// <summary>
/// Observable file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public abstract class ObservableFileFactory<TConfiguration> : BaseDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    protected ObservableFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public override IPropertyCodeTypeReferenceService CodeTypeReferenceService => new ObservableCodeTypeReferenceService<TConfiguration>(this.Configuration);

    public CodeTypeReference BaseObservableAbstractReference => this.Configuration.GetCodeTypeReference(this.Configuration.Environment.DomainObjectBaseType, ObservableFileType.BaseObservableAbstractDTO);

    protected sealed override bool? InternalBaseTypeContainsPropertyChange => true;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = true,
                       IsPartial = true,
                       Attributes = MemberAttributes.Public
               };
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

    protected override IEnumerable<string> GetImportedNamespaces()
    {
        return this.Configuration.GetNamespaces();
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
        if (property.PropertyType.IsCollectionOrArray())
        {
            return new CodeDelegateInvokeExpression(this.GetCodeTypeReference(property, "observableArray`1").ToTypeReferenceExpression());
        }

        var typeReferenceExpression = this.GetCodeTypeReference(property, "observable`1").ToTypeReferenceExpression();
        var defaultAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
        if (defaultAttribute != null)
        {
            return new CodeDelegateInvokeExpression(typeReferenceExpression, new CodePrimitiveExpression(defaultAttribute.Value));
        }

        if (property.PropertyType == typeof(bool))
        {
            return new CodeDelegateInvokeExpression(typeReferenceExpression, new CodePrimitiveExpression(false));
        }

        return new CodeDelegateInvokeExpression(typeReferenceExpression);
    }

    protected override CodeMemberField CreateFieldMember(PropertyInfo property, string fieldName)
    {
        if (property == null)
        {
            throw new ArgumentNullException(nameof(property));
        }

        if (fieldName == null)
        {
            throw new ArgumentNullException(nameof(fieldName));
        }

        return new CodeMemberField
               {
                       Name = fieldName,
                       Type = this.GetCodeTypeReference(property),
                       InitExpression = this.GetFieldInitExpression(property)
               };
    }

    protected CodeMemberMethod GetIdentityObjectContainerImplementation(CodeTypeReference codeRef = null)
    {
        var identityRef = codeRef ?? this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.IdentityDTO);
        var identityObjectContainerImplementation = new CodeMemberMethod
                                                    {
                                                            Attributes = MemberAttributes.Public | MemberAttributes.Final,
                                                            Name = "Identity",
                                                            ReturnType = identityRef
                                                    };

        identityObjectContainerImplementation.Statements
                                             .Add(identityRef.ToObjectCreateExpression(
                                                                                       new CodeThisReferenceExpression()
                                                                                               .ToMethodInvokeExpression(this.Configuration.Environment.IdentityProperty.Name))
                                                             .ToMethodReturnStatement());

        return identityObjectContainerImplementation;
    }

    private CodeTypeReference GetCodeTypeReference(PropertyInfo propertyType, string prefix = "KnockoutObservable`1")
    {
        var codeTypeReference = this.CodeTypeReferenceService.GetCodeTypeReference(propertyType, Constants.UseSecurity);

        if (propertyType.PropertyType.IsCollectionOrArray())
        {
            var elementType = propertyType.PropertyType.GetCollectionOrArrayElementType();

            var typeRef = new CodeTypeReference(elementType.IsEnum ? elementType.Name : elementType.FullName);

            if (codeTypeReference.TypeArguments.Count > 0)
            {
                typeRef = codeTypeReference.TypeArguments[0].CheckForModuleReference(this.Configuration)
                                           .NormalizeTypeReference(elementType);
            }

            if (elementType.IsCollectionOrArray())
            {
                return this.GetPrimitiveTypeReference(elementType, typeRef.NormalizeTypeReference(elementType).ConvertToArray(false), prefix);
            }

            return this.GetPrimitiveTypeReference(elementType, typeRef.NormalizeTypeReference(elementType), prefix);
        }

        var type = propertyType.PropertyType.IsNullable() ? Nullable.GetUnderlyingType(propertyType.PropertyType) : propertyType.PropertyType;

        codeTypeReference = propertyType.PropertyType.IsNullable() ? codeTypeReference.TypeArguments[0].CheckForModuleReference(this.Configuration) : codeTypeReference;

        return this.GetPrimitiveTypeReference(type, codeTypeReference.NormalizeTypeReference(type), prefix);
    }

    private CodeTypeReference GetPrimitiveTypeReference(Type type, CodeTypeReference codeTypeReference, string prefix = null)
    {
        if (this.Configuration.StructTypes.Contains(type))
        {
            return new CodeTypeReference(prefix)
                   {
                           TypeArguments = { codeTypeReference.BaseType }
                   };
        }

        if (type.IsEnum)
        {
            return new CodeTypeReference(prefix)
                   {
                           TypeArguments = { codeTypeReference.BaseType }
                   };
        }

        if (type.IsPeriod())
        {
            return new CodeTypeReference(prefix)
                   {
                           TypeArguments = { new CodeTypeReference(codeTypeReference.BaseType.Replace("Period", "ObservablePeriod")) }
                   };
        }

        if (type.IsClass && !this.IsSystem(type) && !this.IsDomain(type) && !codeTypeReference.BaseType.Contains("Observable"))
        {
            return new CodeTypeReference(prefix)
                   {
                           TypeArguments = { codeTypeReference.BaseType }
                   };
        }

        return new CodeTypeReference(prefix)
               {
                       TypeArguments = { codeTypeReference }
               };
    }

    private bool IsDomain(Type type)
    {
        return type.GetInterfaces().Any(x => x.FullName.Contains("Framework.Persistent.IAuditObject"));
    }

    private bool IsSystem(Type type)
    {
        return type.Namespace != null && type.Namespace.StartsWith("System");
    }
}
