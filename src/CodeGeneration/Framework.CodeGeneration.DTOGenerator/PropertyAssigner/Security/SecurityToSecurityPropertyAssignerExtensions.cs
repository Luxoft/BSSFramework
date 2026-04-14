using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner.Security;

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
