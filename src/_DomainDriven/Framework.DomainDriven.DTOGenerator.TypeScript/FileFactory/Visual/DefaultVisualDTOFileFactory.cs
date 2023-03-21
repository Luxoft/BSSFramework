using System;
using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Visual;

/// <summary>
/// Default visualDTO file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class DefaultVisualDTOFileFactory<TConfiguration> : BaseDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public DefaultVisualDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public override MainDTOFileType FileType => DTOGenerator.FileType.VisualDTO;

    protected override bool? InternalBaseTypeContainsPropertyChange => true;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = true,
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

        yield return this.GenerateDefaultConstructor();

        yield return this.GenerateVisualFromMethodsJs();

        yield return CodeDomHelper.GenerateFromStaticInitializeMethodJs(this);

        if (this.Configuration.GeneratePolicy.Used(this.DomainType, this.FileType.AsObservableFileType()))
        {
            yield return this.GenerateFromObservableMethod();

            yield return this.GenerateToObservableMethod();
        }
    }
}
