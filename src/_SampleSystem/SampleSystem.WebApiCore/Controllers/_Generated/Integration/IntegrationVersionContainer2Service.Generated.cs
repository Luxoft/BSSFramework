namespace SampleSystem.WebApiCore.Controllers.Integration
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/v{version:apiVersion}/[controller]")]
    public partial class IntegrationVersionContainer2Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Save IntegrationVersionContainer2
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveIntegrationVersionContainer2")]
        public virtual SampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveIntegrationVersionContainer2Internal(integrationVersionContainer2, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2Internal(SampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIntegrationVersionContainer2BLL bll = evaluateData.Context.Logics.IntegrationVersionContainer2;
            evaluateData.Context.Authorization.AuthorizationSystem.CheckAccess(Framework.SecuritySystem.Bss.BssSecurityOperation.SystemIntegration);
            return this.SaveIntegrationVersionContainer2Internal(integrationVersionContainer2, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2Internal(SampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IIntegrationVersionContainer2BLL bll)
        {
            SampleSystem.Domain.IntergrationVersions.IntegrationVersionContainer2 domainObject = bll.GetById(integrationVersionContainer2.Id, false, null, Framework.DomainDriven.Lock.LockRole.Update);
            if (object.ReferenceEquals(domainObject, null))
            {
                domainObject = new SampleSystem.Domain.IntergrationVersions.IntegrationVersionContainer2();
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
