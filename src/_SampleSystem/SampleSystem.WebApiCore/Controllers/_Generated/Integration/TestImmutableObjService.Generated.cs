namespace SampleSystem.WebApiCore.Controllers.Integration
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/[controller]/[action]")]
    public partial class TestImmutableObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Save TestImmutableObj
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIntegrationRichDTO testImmutableObj)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveTestImmutableObjInternal(testImmutableObj, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIntegrationRichDTO testImmutableObj, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObj;
            evaluateData.Context.Authorization.SecuritySystem.CheckAccess(Framework.SecuritySystem.SecurityRole.SystemIntegration);
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
