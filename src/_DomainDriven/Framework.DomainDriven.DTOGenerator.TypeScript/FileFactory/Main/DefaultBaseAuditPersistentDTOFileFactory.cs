using System.CodeDom;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main;

public class DefaultBaseAuditPersistentDTOFileFactory<TConfiguration> : BaseDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public DefaultBaseAuditPersistentDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.AuditPersistentDomainObjectBaseType)
    {
    }

    public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.BaseAuditPersistentDTO;

    public CodeTypeReference CurrentInterfaceReference => this.Configuration.GetBaseAuditPersistentInterfaceReference();

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = true,
                       IsPartial = true,
                       TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public,
               };
    }

    protected override bool? InternalBaseTypeContainsPropertyChange { get; }

    protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
    {
        return Constants.InitializeByUndefined ? new CodePrimitiveExpression(null) : null;
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        yield return CodeDomHelper.GenerateFromStaticInitializeMethodJs(this);

        yield return this.GenerateToStrictMethods(this.FileType);

        yield return this.GenerateFromMethodsJs();

        if (this.Configuration.GeneratePolicy.Used(this.DomainType, this.FileType.AsObservableFileType()))
        {
            yield return this.GenerateFromObservableMethod();
        }
    }
}
