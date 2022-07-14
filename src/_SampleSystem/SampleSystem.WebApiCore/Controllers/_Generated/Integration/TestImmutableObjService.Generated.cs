﻿namespace SampleSystem.WebApiCore.Controllers.Integration
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/v{version:apiVersion}/[controller]")]
    public partial class TestImmutableObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public TestImmutableObjController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Save TestImmutableObj
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveTestImmutableObj")]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIntegrationRichDTO testImmutableObj)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveTestImmutableObjInternal(testImmutableObj, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIntegrationRichDTO testImmutableObj, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObj;
            Framework.DomainDriven.BLL.Security.AuthorizationBLLContextExtensions.CheckAccess(evaluateData.Context.Authorization, SampleSystem.BLL.SampleSystemSecurityOperation.SystemIntegration);
            return this.SaveTestImmutableObjInternal(testImmutableObj, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIntegrationRichDTO testImmutableObj, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ITestImmutableObjBLL bll)
        {
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetById(testImmutableObj.Id, false);
            if (object.ReferenceEquals(domainObject, null))
            {
                domainObject = new SampleSystem.Domain.TestImmutableObj();
            }
            testImmutableObj.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Insert(domainObject, testImmutableObj.Id);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
