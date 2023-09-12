namespace SampleSystem.WebApiCore.Controllers.Integration
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/v{version:apiVersion}/[controller]")]
    public partial class TestImmutableObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Save TestImmutableObj
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveTestImmutableObj")]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIntegrationRichDTO testImmutableObj)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveTestImmutableObjInternal(testImmutableObj, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIntegrationRichDTO testImmutableObj, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObj;
            evaluateData.Context.Authorization.CheckAccess(Framework.Core.BssSecurityOperation.SystemIntegration);
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
