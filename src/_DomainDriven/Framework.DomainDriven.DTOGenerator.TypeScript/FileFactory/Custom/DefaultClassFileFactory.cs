using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Custom;

/// <summary>
/// Default class file factory  configuration
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class DefaultClassFileFactory<TConfiguration> : PropertyFileFactory<TConfiguration, DTOFileType>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public DefaultClassFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public override DTOFileType FileType => ClientFileType.Class;

    public override CodeTypeReference BaseReference => this.HasBase
                                                               ? this.Configuration.GetCodeTypeReference(this.DomainType.BaseType, ClientFileType.Class)
                                                               : null;

    protected override bool? InternalBaseTypeContainsPropertyChange => this.HasBase;

    private bool HasBase => this.DomainType.BaseType != typeof(object);

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var classDeclaration = new CodeTypeDeclaration(this.Name)
                               {
                                       IsClass = true,
                                       IsPartial = true,
                                       Attributes = MemberAttributes.Public
                               };

        return classDeclaration;
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }
    }

    protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
    {
        if (this.CodeTypeReferenceService.IsOptional(property) || !property.PropertyType.IsCollectionOrArray())
        {
            return base.GetFieldInitExpression(property);
        }

        var type = property.PropertyType.GetCollectionOrArrayElementType();

        if (property.PropertyType.IsCollection())
        {
            return new CodeArrayCreateExpression(new CodeTypeReference(type.Name.GetLastKeyword().ConvertToTypeScriptType(true)).ConvertToArray());
        }

        var reference = new CodeTypeReference(property.PropertyType.FullName.SkipLast("[]", true));

        return new CodeArrayCreateExpression(reference.NormalizeTypeReference(type).ConvertToArray());
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        yield return CodeDomHelper.GenerateFromStaticInitializeMethodJs(this);
        yield return this.GenerateFromMethodsJs();
        yield return this.GenerateToNativeJsonMethod();
        yield return this.GenerateSelfToJson();
    }
}
