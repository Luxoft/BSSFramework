namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/[controller]/[action]")]
    public partial class EmployeeQueryController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Employees (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullEmployeesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeFullDTO> GetFullEmployeesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.Employee.Employee> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Employee.Employee>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.Employee.Employee> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.Employee>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Employees (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeSimpleDTO> GetSimpleEmployeesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.Employee.Employee> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Employee.Employee>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.Employee.Employee> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.Employee>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.EmployeeSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestManualEmployeeProjections (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestManualEmployeeProjectionDTO> GetTestManualEmployeeProjectionsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetTestManualEmployeeProjectionsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestManualEmployeeProjectionDTO> GetTestManualEmployeeProjectionsByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestManualEmployeeProjectionBLL bll = evaluateData.Context.Logics.TestManualEmployeeProjectionFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.ManualProjections.TestManualEmployeeProjection> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.ManualProjections.TestManualEmployeeProjection>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.ManualProjections.TestManualEmployeeProjection> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.ManualProjections.TestManualEmployeeProjection>(Framework.BLL.Domain.DTO.ViewDTOType.ProjectionDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestManualEmployeeProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestEmployees (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO> GetTestEmployeesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetTestEmployeesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO> GetTestEmployeesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestEmployeeBLL bll = evaluateData.Context.Logics.TestEmployeeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.Projections.TestEmployee> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Projections.TestEmployee>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.Projections.TestEmployee> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Projections.TestEmployee>(Framework.BLL.Domain.DTO.ViewDTOType.ProjectionDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestEmployees (ProjectionDTO) by odata string and filter (TestEmployeeFilter)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO> GetTestEmployeesByODataQueryStringWithFilter(GetTestEmployeesByODataQueryStringWithFilterAutoRequest getTestEmployeesByODataQueryStringWithFilterAutoRequest)
        {
            SampleSystem.Generated.DTO.TestEmployeeFilterStrictDTO filter = getTestEmployeesByODataQueryStringWithFilterAutoRequest.Filter;
            string odataQueryString = getTestEmployeesByODataQueryStringWithFilterAutoRequest.OdataQueryString;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetTestEmployeesByODataQueryStringWithFilterInternal(odataQueryString, filter, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO> GetTestEmployeesByODataQueryStringWithFilterInternal(string odataQueryString, SampleSystem.Generated.DTO.TestEmployeeFilterStrictDTO filter, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestEmployeeBLL bll = evaluateData.Context.Logics.TestEmployeeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.Projections.TestEmployee> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Projections.TestEmployee>(odataQueryString);
            SampleSystem.Domain.Models.Filters.Projection.TestEmployeeFilter typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.Projections.TestEmployee> preResult = bll.GetObjectsByOData(selectOperation, typedFilter, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Projections.TestEmployee>(Framework.BLL.Domain.DTO.ViewDTOType.ProjectionDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestEmployeeProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestLegacyEmployees (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestLegacyEmployeeProjectionDTO> GetTestLegacyEmployeesByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetTestLegacyEmployeesByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestLegacyEmployeeProjectionDTO> GetTestLegacyEmployeesByODataQueryStringInternal(string odataQueryString, Framework.Infrastructure.Services.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLegacyEmployeeBLL bll = evaluateData.Context.Logics.TestLegacyEmployeeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            Anch.OData.Domain.SelectOperation<SampleSystem.Domain.Projections.TestLegacyEmployee> selectOperation = evaluateData.Context.SelectOperationParser.Parse<SampleSystem.Domain.Projections.TestLegacyEmployee>(odataQueryString);
            Anch.OData.Domain.SelectOperationResult<SampleSystem.Domain.Projections.TestLegacyEmployee> preResult = bll.GetObjectsByOData(selectOperation, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Projections.TestLegacyEmployee>(Framework.BLL.Domain.DTO.ViewDTOType.ProjectionDTO));
            return new Anch.OData.Domain.SelectOperationResult<SampleSystem.Generated.DTO.TestLegacyEmployeeProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetTestEmployeesByODataQueryStringWithFilterAutoRequest
    {
        
        private string odataQueryString;
        
        private SampleSystem.Generated.DTO.TestEmployeeFilterStrictDTO filter;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual string OdataQueryString
        {
            get
            {
                return this.odataQueryString;
            }
            set
            {
                this.odataQueryString = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual SampleSystem.Generated.DTO.TestEmployeeFilterStrictDTO Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                this.filter = value;
            }
        }
    }
}
