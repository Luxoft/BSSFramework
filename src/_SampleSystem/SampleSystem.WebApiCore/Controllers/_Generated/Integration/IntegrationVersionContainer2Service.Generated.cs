﻿namespace SampleSystem.WebApiCore.Controllers.Integration
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/[controller]/[action]")]
    public partial class IntegrationVersionContainer2Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Save IntegrationVersionContainer2
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveIntegrationVersionContainer2Internal(integrationVersionContainer2, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2Internal(SampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIntegrationVersionContainer2BLL bll = evaluateData.Context.Logics.IntegrationVersionContainer2;
            evaluateData.Context.Authorization.SecuritySystem.CheckAccess(Framework.SecuritySystem.SecurityRole.SystemIntegration);
            return this.SaveIntegrationVersionContainer2Internal(integrationVersionContainer2, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2Internal(SampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IIntegrationVersionContainer2BLL bll)
        {
            SampleSystem.Domain.IntegrationVersions.IntegrationVersionContainer2 domainObject = bll.GetById(integrationVersionContainer2.Id, false, null, Framework.DomainDriven.Lock.LockRole.Update);
            if (object.ReferenceEquals(domainObject, null))
            {
                domainObject = new SampleSystem.Domain.IntegrationVersions.IntegrationVersionContainer2();
            }
            if ((domainObject.IntegrationVersion <= integrationVersionContainer2.IntegrationVersion))
            {
                integrationVersionContainer2.MapToDomainObject(evaluateData.MappingService, domainObject);
                bll.Insert(domainObject, integrationVersionContainer2.Id);
            }
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
