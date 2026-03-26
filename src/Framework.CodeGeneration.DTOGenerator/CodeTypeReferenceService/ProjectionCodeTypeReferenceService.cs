using System.Reflection;

using CommonFramework;

using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileType;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class ProjectionCodeTypeReferenceService<TConfiguration> : MainCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public ProjectionCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
    {
    }


    public override Type CollectionType => this.Configuration.ClientEditCollectionType;


    public override RoleFileType GetReferenceFileType(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        if (this.Configuration.ProjectionTypes.Contains(property.PropertyType))
        {
            return FileType.FileType.ProjectionDTO;
        }
        else
        {
            return base.GetReferenceFileType(property);
        }
    }

    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        if (this.Configuration.ProjectionTypes.Contains(property.PropertyType.GetCollectionElementType()))
        {
            return FileType.FileType.ProjectionDTO;
        }
        else
        {
            return base.GetReferenceFileType(property);
        }
    }
}
