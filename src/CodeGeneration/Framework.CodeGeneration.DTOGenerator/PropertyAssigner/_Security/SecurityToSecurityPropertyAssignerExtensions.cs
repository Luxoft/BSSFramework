using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner._Security;

public static class SecurityToSecurityPropertyAssignerExtensions
{
    public static IPropertyAssigner WithSecurityToSecurity<TConfiguration>(this IPropertyAssigner<TConfiguration> innerAssigner, IPropertyCodeTypeReferenceService sourceTypeReferenceService)
            where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
    {
        if (innerAssigner == null) throw new ArgumentNullException(nameof(innerAssigner));
        if (sourceTypeReferenceService == null) throw new ArgumentNullException(nameof(sourceTypeReferenceService));

        return new SecurityToSecurityPropertyAssigner<TConfiguration>(innerAssigner, sourceTypeReferenceService);
    }
}
