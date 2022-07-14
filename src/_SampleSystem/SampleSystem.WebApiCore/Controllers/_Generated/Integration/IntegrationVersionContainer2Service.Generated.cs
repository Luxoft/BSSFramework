namespace SampleSystem.WebApiCore.Controllers.Integration
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/v{version:apiVersion}/[controller]")]
    public partial class IntegrationVersionContainer2Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public IntegrationVersionContainer2Controller(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Save IntegrationVersionContainer2
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveIntegrationVersionContainer2")]
        public virtual SampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveIntegrationVersionContainer2Internal(integrationVersionContainer2, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2Internal(SampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIntegrationVersionContainer2BLL bll = evaluateData.Context.Logics.IntegrationVersionContainer2;
            Framework.DomainDriven.BLL.Security.AuthorizationBLLContextExtensions.CheckAccess(evaluateData.Context.Authorization, SampleSystem.BLL.SampleSystemSecurityOperation.SystemIntegration);
            return this.SaveIntegrationVersionContainer2Internal(integrationVersionContainer2, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.IntegrationVersionContainer2IdentityDTO SaveIntegrationVersionContainer2Internal(SampleSystem.Generated.DTO.IntegrationVersionContainer2IntegrationRichDTO integrationVersionContainer2, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IIntegrationVersionContainer2BLL bll)
        {
            SampleSystem.Domain.IntergrationVersions.IntegrationVersionContainer2 domainObject = bll.GetById(integrationVersionContainer2.Id, false, null, Framework.DomainDriven.LockRole.Update);
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
