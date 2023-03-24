using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;

/// <summary>
/// IProperty file factory interface
/// </summary>
public interface IPropertyFileFactory : IClientFileFactory
{
    IEnumerable<PropertyInfo> GetProperties(bool isWritable = false);
}

/// <summary>
/// IProperty file factory interface
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
/// <typeparam name="TFileType">The type of the file type.</typeparam>
public interface IPropertyFileFactory<out TConfiguration, out TFileType> : IPropertyFileFactory, IClientFileFactory<TConfiguration, TFileType>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
        where TFileType : DTOFileType
{
}

/// <summary>
/// Property file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
/// <typeparam name="TFileType">The type of the file type.</typeparam>
public abstract class PropertyFileFactory<TConfiguration, TFileType> : ClientFileFactory<TConfiguration, TFileType>, IPropertyFileFactory<TConfiguration, TFileType>
        where TFileType : DTOFileType
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    protected PropertyFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    protected abstract bool? InternalBaseTypeContainsPropertyChange { get; }

    private bool? BaseTypeContainsPropertyChange => this.Configuration.ContainsPropertyChange ? this.InternalBaseTypeContainsPropertyChange : null;

    public IEnumerable<PropertyInfo> GetProperties(bool isWritable = false)
    {
        return this.Configuration.GetDomainTypeProperties(this.DomainType, this.FileType, isWritable);
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        foreach (var notifyMember in this.CreatePropertyMembers())
        {
            yield return notifyMember;
        }
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        foreach (var customAttribute in base.GetCustomAttributes())
        {
            yield return customAttribute;
        }

        yield return this.GetDataContractCodeAttributeDeclaration();
    }

    protected virtual CodeMemberField CreateFieldMember(PropertyInfo property, string fieldName)
    {
        if (property == null) { throw new ArgumentNullException(nameof(property)); }
        if (fieldName == null) { throw new ArgumentNullException(nameof(fieldName)); }

        return new CodeMemberField
               {
                       Name = fieldName,
                       Type = this.CodeTypeReferenceService.GetCodeTypeReference(property, Constants.UseSecurity),
                       InitExpression = this.GetFieldInitExpression(property)
               };
    }

    protected virtual CodeExpression GetFieldInitExpression(PropertyInfo property)
    {
        return property.GetCustomAttribute<DefaultValueAttribute>().Maybe(attr => attr.Value.ToDynamicPrimitiveExpression());
    }

    protected virtual IEnumerable<CodeAttributeDeclaration> GetPropertyCustomAttributes(PropertyInfo sourceProperty)
    {
        if (sourceProperty == null)
        {
            throw new ArgumentNullException(nameof(sourceProperty));
        }

        yield return typeof(DataMemberAttribute).ToTypeReference().ToAttributeDeclaration();
    }

    private IEnumerable<CodeTypeMember> CreatePropertyMembers()
    {
        return from sourceProperty in this.GetProperties()
               from member in this.CreatePropertyMember(sourceProperty)
               select member;
    }

    private IEnumerable<CodeTypeMember> CreatePropertyMember(PropertyInfo sourceProperty)
    {
        if (sourceProperty == null)
        {
            throw new ArgumentNullException(nameof(sourceProperty));
        }

        if (this.BaseTypeContainsPropertyChange == null)
        {
            yield return this.CreateFieldMember(sourceProperty, sourceProperty.Name)
                             .Self(field => field.CustomAttributes.Add(typeof(DataMemberAttribute).ToTypeReference().ToAttributeDeclaration()))
                             .Self(field => field.Attributes = MemberAttributes.Public);
        }
        else
        {
            var fieldMember = this.CreateFieldMember(sourceProperty, "_" + sourceProperty.Name.ToStartLowerCase());
            yield return fieldMember;
        }
    }
}
