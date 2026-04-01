using System.CodeDom;
using System.Reflection;
using System.Runtime.Serialization;

using CommonFramework;

using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

public abstract class FileFactory<TConfiguration, TFileType> : CodeFileFactory<TConfiguration, TFileType>, IFileFactory<TConfiguration, TFileType>
        where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
        where TFileType : BaseFileType
{
    protected FileFactory(TConfiguration configuration, Type? domainType)
            : base(configuration, domainType) =>
        this.CodeTypeReferenceService = new PropertyCodeTypeReferenceService<TConfiguration>(this.Configuration);

    public string FileTypeName => this.FileType.Name;

    public virtual IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }


    protected CodeAttributeDeclaration GetDataContractCodeAttributeDeclaration(string? overrideNamespace = null)
    {
        var attr = this.DomainType.Maybe(domainType => domainType.GetCustomAttribute<DataContractAttribute>());

        return attr == null
                       ? new CodeAttributeDeclaration(new CodeTypeReference(typeof(DataContractAttribute)),
                                                      new CodeAttributeArgument("Namespace", new CodePrimitiveExpression(overrideNamespace ?? this.Configuration.DataContractNamespace)))

                       : new CodeAttributeDeclaration(new CodeTypeReference(typeof(DataContractAttribute)),

                                                      new[]
                                                      {
                                                              attr.Name.Maybe(name => new CodeAttributeArgument("Name", new CodePrimitiveExpression(name))),
                                                              attr.Namespace.Maybe(ns => new CodeAttributeArgument("Namespace", new CodePrimitiveExpression(ns)))
                                                      }.Where(v => v != null).ToArray());
    }
}
