namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class ManagementUnitAndHRDepartmentLinkQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLinks (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinksByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLinks (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinksByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
}
