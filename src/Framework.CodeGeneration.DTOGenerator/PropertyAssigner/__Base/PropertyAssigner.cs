using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeGeneration.Configuration._Container;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.FileType;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;

public abstract class PropertyAssigner<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IPropertyAssigner<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected PropertyAssigner(IDTOSource<TConfiguration> source)
            : this(source.Configuration, source.DomainType!, source.FileType)
    {
    }


    protected PropertyAssigner(TConfiguration configuration, Type domainType, DTOFileType fileType)
            : base(configuration)
    {
        this.DomainType = domainType;
        this.FileType = fileType;

        this.CodeTypeReferenceService = this.Configuration.GetLayerCodeTypeReferenceService(fileType);
    }


    public Type DomainType { get; }

    public DTOFileType FileType { get; }


    protected ILayerCodeTypeReferenceService CodeTypeReferenceService { get; }


    public virtual CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

        return sourcePropertyRef.ToAssignStatement(targetPropertyRef);
    }
}
