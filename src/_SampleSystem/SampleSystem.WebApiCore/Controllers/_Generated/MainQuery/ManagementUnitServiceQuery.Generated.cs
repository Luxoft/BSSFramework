namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class ManagementUnitQueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get ManagementUnits (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.MU.ManagementUnit> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.MU.ManagementUnit>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.MU.ManagementUnit> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnit>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get ManagementUnits (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.MU.ManagementUnit> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.MU.ManagementUnit>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.MU.ManagementUnit> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnit>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get ManagementUnits (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.MU.ManagementUnit> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.MU.ManagementUnit>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.MU.ManagementUnit> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.MU.ManagementUnit>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
