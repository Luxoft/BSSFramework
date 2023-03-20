using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main;

/// <summary>
/// Default richDTO file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class DefaultRichDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public DefaultRichDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public override MainDTOFileType FileType => DTOGenerator.FileType.RichDTO;

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
                       Type = this.SimplifyCodeTypeReference(property),
                       InitExpression = this.GetFieldInitExpression(property)
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        foreach (var customAttribute in base.GetCustomAttributes())
        {
            yield return customAttribute;
        }

        foreach (var attributeDeclaration in this.DomainType.GetRestrictionCodeAttributeDeclarations())
        {
            yield return attributeDeclaration;
        }
    }
}
