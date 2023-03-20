using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;

/// <summary>
/// Default strictDTO file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class DefaultStrictDTOFileFactory<TConfiguration> : PropertyFileFactory<TConfiguration, DTOFileType>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public DefaultStrictDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public override DTOFileType FileType => DTOGenerator.FileType.StrictDTO;

    public override IPropertyCodeTypeReferenceService CodeTypeReferenceService => new StrictCodeTypeReferenceService<TConfiguration>(this.Configuration);

    protected override bool? InternalBaseTypeContainsPropertyChange => null;

    protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
    {
        return Constants.InitializeByUndefined ? new CodePrimitiveExpression(null) : null;
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
                       Type = this.SimplifyCodeTypeReference(property),
                       InitExpression = this.GetFieldInitExpression(property)
               };
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

        foreach (var fileType in this.GetFileTypes())
        {
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, fileType))
            {
                yield return CodeDomHelper.GenerateFromStrictStaticInitializeMethodJs(this, fileType, this.FileType);
            }
        }

        yield return this.GenerateToNativeJsonMethod();

        foreach (var fileType in this.GetFileTypes())
        {
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, fileType))
            {
                yield return this.GenerateToStrictMethods(fileType);
            }
        }
    }

    private IEnumerable<MainDTOFileType> GetFileTypes()
    {
        yield return DTOGenerator.FileType.RichDTO;
        yield return DTOGenerator.FileType.FullDTO;
        yield return DTOGenerator.FileType.SimpleDTO;

        if (this.IsPersistent())
        {
            yield return DTOGenerator.FileType.BaseAuditPersistentDTO;
            yield return DTOGenerator.FileType.BasePersistentDTO;
        }
    }
}
