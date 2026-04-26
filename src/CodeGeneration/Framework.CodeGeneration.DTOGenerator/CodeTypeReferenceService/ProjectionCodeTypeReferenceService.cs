using System.Reflection;

using Anch.Core;

using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class ProjectionCodeTypeReferenceService<TConfiguration>(TConfiguration configuration) : MainCodeTypeReferenceService<TConfiguration>(configuration)
    where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    public override Type CollectionType => this.Configuration.ClientEditCollectionType;


    public override RoleFileType GetReferenceFileType(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        if (this.Configuration.ProjectionTypes.Contains(property.PropertyType))
        {
            return BaseFileType.ProjectionDTO;
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
            return BaseFileType.ProjectionDTO;
        }
        else
        {
            return base.GetReferenceFileType(property);
        }
    }
}
