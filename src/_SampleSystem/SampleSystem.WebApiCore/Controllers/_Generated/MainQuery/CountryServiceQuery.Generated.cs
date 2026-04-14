namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class CountryQueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Countries (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullCountriesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.Directories.Country> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Directories.Country>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.Directories.Country> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CountryFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Countries (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleCountriesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.Directories.Country> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Directories.Country>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.Directories.Country> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CountrySimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Countries (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualCountriesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            OData.Domain.SelectOperation<SampleSystem.Domain.Directories.Country> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Directories.Country>(odataQueryString);
            OData.Domain.SelectOperationResult<SampleSystem.Domain.Directories.Country> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Directories.Country>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return new OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.CountryVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
