using System.CodeDom;

using Framework.Core;

namespace Framework.DomainDriven.Generation.Domain;

public abstract class CodeFileFactory<TConfiguration, TFileType>(TConfiguration configuration, Type? domainType)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), ICodeFileFactory<TFileType>
    where TConfiguration : class, IGeneratorConfiguration<IGenerationEnvironment, TFileType>
{
    public string Name => this.Header.GetName(this.DomainType);

    public virtual CodeTypeReference CurrentReference => this.Configuration.GetCodeTypeReference(this.DomainType, this.FileType);

    public abstract TFileType FileType { get; }

    public virtual CodeTypeReference? BaseReference { get; } = null;


    public ICodeFileFactoryHeader<TFileType> Header => (ICodeFileFactoryHeader<TFileType>)this.Configuration.GetFileFactoryHeader(this.FileType)!;

    public Type? DomainType { get; } = domainType;

    protected abstract CodeTypeDeclaration GetCodeTypeDeclaration();


    protected virtual IEnumerable<string> GetImportedNamespaces()
    {
        yield break;
    }


    protected virtual IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        return this.BaseReference.MaybeYield();
    }

    protected virtual IEnumerable<CodeTypeMember> GetMembers()
    {
        return this.GetConstructors();
    }

    protected virtual IEnumerable<CodeConstructor> GetConstructors()
    {
        yield break;
    }

    protected virtual IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield break;
    }

    protected CodeTypeDeclaration GetCompleteCodeTypeDeclaration()
    {
        return this.GetCodeTypeDeclaration().Self(decl =>
                                                  {
                                                      decl.BaseTypes.AddRange(this.GetBaseTypes().ToArray());

                                                      decl.Members.AddRange(this.GetMembers()
                                                                                .OrderBy(x => x.Attributes.HasFlag(MemberAttributes.Const) ? 0 : 1)
                                                                                .ThenBy(x => x is CodeConstructor ? 0 : 1)
                                                                                .ThenBy(x => x.Attributes.HasFlag(MemberAttributes.Static) ? 0 : 1)
                                                                                .ThenBy(x => x.Name).ToArray());

                                                      decl.CustomAttributes.AddRange(this.GetCustomAttributes().OrderBy(x => x.Name).ToArray());

                                                      decl.UserData["DomainType"] = this.DomainType;
                                                      decl.UserData["FileType"] = this.FileType;
                                                  });
    }


    public CodeNamespace GetRenderData()
    {
        return new CodeNamespace(this.Configuration.Namespace)
               {
                       Types = { this.GetCompleteCodeTypeDeclaration() }
               }.Self(v => v.Imports.AddRange(this.GetImportedNamespaces().ToArray(n => new CodeNamespaceImport(n))));
    }

    #region IFileFactory Members

    string IFileHeader.Filename => this.Name;

    #endregion
}
