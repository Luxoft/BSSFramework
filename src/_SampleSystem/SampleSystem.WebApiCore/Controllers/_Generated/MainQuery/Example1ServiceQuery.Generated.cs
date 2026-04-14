namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class Example1QueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Example1s (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.Example1FullDTO> GetFullExample1sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullExample1sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.Example1FullDTO> GetFullExample1sByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.ForUpdate.Example1> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.ForUpdate.Example1>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.ForUpdate.Example1> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ForUpdate.Example1>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.Example1FullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Example1s (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.Example1SimpleDTO> GetSimpleExample1sByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleExample1sByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.Example1SimpleDTO> GetSimpleExample1sByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.ForUpdate.Example1> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.ForUpdate.Example1>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.ForUpdate.Example1> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ForUpdate.Example1>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.Example1SimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
