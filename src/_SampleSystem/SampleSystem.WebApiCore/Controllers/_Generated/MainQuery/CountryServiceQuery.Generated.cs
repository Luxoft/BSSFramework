namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class CountryQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Countries (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCountriesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CountryFullDTO> GetFullCountriesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Country> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Country>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Country> preResult = bll.GetObjectsByOData(typedSelectOperation, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CountryFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Countries (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCountriesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CountrySimpleDTO> GetSimpleCountriesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Country> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Country>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Country> preResult = bll.GetObjectsByOData(typedSelectOperation, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CountrySimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Countries (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCountriesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CountryVisualDTO> GetVisualCountriesByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Country> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Country>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Country> preResult = bll.GetObjectsByOData(typedSelectOperation, new Framework.DomainDriven.DTOFetchRule<SampleSystem.Domain.Country>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.CountryVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
