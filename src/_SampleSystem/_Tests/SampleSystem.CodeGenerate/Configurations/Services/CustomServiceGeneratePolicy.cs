using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.DTO;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.GeneratePolicy;


using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

/// <summary>
/// Кастомная политика для управления генерацией фасадных методов (пример для обработки генерируемых методов по ComplexChange-модели)
/// </summary>
public class CustomServiceGeneratePolicy(IServiceModelGenerationEnvironment generationEnvironment) : DefaultServiceGeneratePolicy(generationEnvironment)
{
    public override bool Used(Type domainType, MethodIdentity identity)
    {
        if (identity.Type == MethodIdentityType.GetList && domainType.Name == nameof(SampleSystemProjectionSource.TestSecurityObjItemProjection))
        {
            return true;
        }

        if (domainType == typeof(ManagementUnit) && identity.Type == MethodIdentityType.GetTreeByOperation && identity.DTOType == ViewDTOType.FullDTO)
        {
            return true;
        }

        if (domainType == typeof(BusinessUnit) && identity.Type == MethodIdentityType.HasAccess)
        {
            return true;
        }

        if (identity == SampleSystemMethodIdentityType.ComplexChange)
        {
            return identity.ModelType.GetDirectMode().HasFlag(DirectMode.In);
        }

        return base.Used(domainType, identity);
    }
}
