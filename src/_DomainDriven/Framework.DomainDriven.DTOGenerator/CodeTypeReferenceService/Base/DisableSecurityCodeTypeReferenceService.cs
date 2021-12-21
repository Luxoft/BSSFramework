using System.Reflection;

namespace Framework.DomainDriven.DTOGenerator
{
    public class DisableSecurityCodeTypeReferenceService<TConfiguration> : PropertyCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public DisableSecurityCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
        {
        }


        public override bool IsOptional(PropertyInfo property)
        {
            return false;
        }
    }
}