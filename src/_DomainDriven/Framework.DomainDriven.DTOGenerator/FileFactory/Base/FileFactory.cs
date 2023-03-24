using System;
using System.CodeDom;
using System.Linq;
using System.Runtime.Serialization;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator;

public abstract class FileFactory<TConfiguration, TFileType> : CodeFileFactory<TConfiguration, TFileType>, IFileFactory<TConfiguration, TFileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
        where TFileType : FileType
{
    protected FileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.CodeTypeReferenceService = new PropertyCodeTypeReferenceService<TConfiguration>(this.Configuration);
    }


    public string FileTypeName => this.FileType.Name;

    public virtual IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }


    protected CodeAttributeDeclaration GetDataContractCodeAttributeDeclaration(string overrideNamespace = null)
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
