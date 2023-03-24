using System;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner.Security;

/// <summary>
/// Maybe To security property assigner extensions
/// </summary>
public static class MaybeToSecurityPropertyAssignerExtensions
{
    public static IPropertyAssigner MaybeSecurityToSecurity<TConfiguration>(this IPropertyAssigner<TConfiguration> innerAssigner, IPropertyCodeTypeReferenceService sourceTypeReferenceService, SecurityDirection direction)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (innerAssigner == null)
        {
            throw new ArgumentNullException(nameof(innerAssigner));
        }

        if (sourceTypeReferenceService == null)
        {
            throw new ArgumentNullException(nameof(sourceTypeReferenceService));
        }

        return new MaybeToSecurityPropertyAssigner<TConfiguration>(innerAssigner, sourceTypeReferenceService, direction);
    }
}
