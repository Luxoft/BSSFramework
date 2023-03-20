using System;
using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Custom;

/// <summary>
/// Default struct file factory configuration
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class DefaultStructFileFactory<TConfiguration> : PropertyFileFactory<TConfiguration, DTOFileType>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public DefaultStructFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public override DTOFileType FileType => ClientFileType.Struct;

    public override CodeTypeReference BaseReference => null;

    protected override bool? InternalBaseTypeContainsPropertyChange => false;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsStruct = true,
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

        yield return CodeDomHelper.GenerateFromStaticInitializeMethodJs(this);
        yield return this.GenerateFromMethodsJs();
        yield return this.GenerateToNativeJsonMethod();
        yield return this.GenerateSelfToJson();
    }
}
