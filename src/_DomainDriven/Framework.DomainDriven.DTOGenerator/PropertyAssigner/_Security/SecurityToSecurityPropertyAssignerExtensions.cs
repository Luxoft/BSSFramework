namespace Framework.DomainDriven.DTOGenerator;

public static class SecurityToSecurityPropertyAssignerExtensions
{
    public static IPropertyAssigner WithSecurityToSecurity<TConfiguration>(this IPropertyAssigner<TConfiguration> innerAssigner, IPropertyCodeTypeReferenceService sourceTypeReferenceService)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (innerAssigner == null) throw new ArgumentNullException(nameof(innerAssigner));
        if (sourceTypeReferenceService == null) throw new ArgumentNullException(nameof(sourceTypeReferenceService));

        return new SecurityToSecurityPropertyAssigner<TConfiguration>(innerAssigner, sourceTypeReferenceService);
    }
}
