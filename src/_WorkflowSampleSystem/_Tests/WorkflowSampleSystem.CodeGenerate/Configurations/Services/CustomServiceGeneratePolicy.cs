using System;

using Framework.DomainDriven;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.Transfering;

using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.CodeGenerate
{
    /// <summary>
    /// Кастомная политика для управления генерацией фасадных методов (пример для обработки генерируемых методов по ComplexChange-модели)
    /// </summary>
    public class CustomServiceGeneratePolicy : DefaultServiceGeneratePolicy
    {
        public override bool Used(Type domainType, MethodIdentity identity)
        {
            if (identity.Type == MethodIdentityType.GetList && domainType.Name == nameof(WorkflowSampleSystemProjectionSource.TestSecurityObjItemProjection))
            {
                return true;
            }

            if (domainType == typeof(ManagementUnit) && identity.Type == MethodIdentityType.GetTreeByOperation && identity.DTOType == ViewDTOType.FullDTO)
            {
                return true;
            }

            if (identity == WorkflowSampleSystemMethodIdentityType.ComplexChange)
            {
                return identity.ModelType.GetDirectMode().HasFlag(DirectMode.In);
            }

            return base.Used(domainType, identity);
        }
    }
}
