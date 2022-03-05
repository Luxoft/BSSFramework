namespace SampleSystem.WebApiCore.Controllers.MainQuery
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainQueryApi/v{version:apiVersion}/[controller]")]
    public partial class LocationQueryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public LocationQueryController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get Locations (FullDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullLocationsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.LocationFullDTO> GetFullLocationsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullLocationsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.LocationFullDTO> GetFullLocationsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Location> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Location>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Location> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.FullDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.LocationFullDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Locations (SimpleDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleLocationsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.LocationSimpleDTO> GetSimpleLocationsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleLocationsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.LocationSimpleDTO> GetSimpleLocationsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Location> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Location>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Location> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.LocationSimpleDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get Locations (VisualDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualLocationsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.LocationVisualDTO> GetVisualLocationsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualLocationsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.LocationVisualDTO> GetVisualLocationsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Location> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Location>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Location> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Location>(Framework.Transfering.ViewDTOType.VisualDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.LocationVisualDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get TestLocations (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestLocationsByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationProjectionDTO> GetTestLocationsByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestLocationsByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationProjectionDTO> GetTestLocationsByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLocationBLL bll = evaluateData.Context.Logics.TestLocationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestLocation> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestLocation>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Projections.TestLocation> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestLocation>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get hierarchical data of type TestLocations (ProjectionDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestLocationsByOperation")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationProjectionDTO> GetTestLocationsByOperation(GetTestLocationsByOperationAutoRequest getTestLocationsByOperationAutoRequest)
        {
            SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode = getTestLocationsByOperationAutoRequest.securityOperationCode;
            string odataQueryString = getTestLocationsByOperationAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestLocationsByOperationInternal(odataQueryString, securityOperationCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationProjectionDTO> GetTestLocationsByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLocationBLL bll = evaluateData.Context.Logics.TestLocationFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestLocation> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestLocation>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataList = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestLocation>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.Select(odataList, testLocation => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(testLocation, evaluateData.MappingService));
        }
        
        /// <summary>
        /// Get hierarchical data of type TestLocations (ProjectionDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestLocationTreeByOperation")]
        public virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestLocationProjectionDTO, System.Guid>> GetTestLocationTreeByOperation(GetTestLocationTreeByOperationAutoRequest getTestLocationTreeByOperationAutoRequest)
        {
            SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode = getTestLocationTreeByOperationAutoRequest.securityOperationCode;
            string odataQueryString = getTestLocationTreeByOperationAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestLocationTreeByOperationInternal(odataQueryString, securityOperationCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestLocationProjectionDTO, System.Guid>> GetTestLocationTreeByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLocationBLL bll = evaluateData.Context.Logics.TestLocationFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestLocation> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestLocation>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataTree = bll.GetTreeByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestLocation>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.SelectN(odataTree, testLocation => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(testLocation, evaluateData.MappingService));
        }
        
        /// <summary>
        /// Get TestLocationCollectionPropertiess (ProjectionDTO) by odata string
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestLocationCollectionPropertiessByODataQueryString")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationCollectionPropertiesProjectionDTO> GetTestLocationCollectionPropertiessByODataQueryString([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string odataQueryString)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestLocationCollectionPropertiessByODataQueryStringInternal(odataQueryString, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationCollectionPropertiesProjectionDTO> GetTestLocationCollectionPropertiessByODataQueryStringInternal(string odataQueryString, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLocationCollectionPropertiesBLL bll = evaluateData.Context.Logics.TestLocationCollectionPropertiesFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestLocationCollectionProperties> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestLocationCollectionProperties>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            Framework.OData.SelectOperationResult<SampleSystem.Domain.Projections.TestLocationCollectionProperties> preResult = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestLocationCollectionProperties>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return new Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationCollectionPropertiesProjectionDTO>(SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTOList(preResult.Items, evaluateData.MappingService), preResult.TotalCount);
        }
        
        /// <summary>
        /// Get hierarchical data of type TestLocationCollectionPropertiess (ProjectionDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestLocationCollectionPropertiessByOperation")]
        public virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationCollectionPropertiesProjectionDTO> GetTestLocationCollectionPropertiessByOperation(GetTestLocationCollectionPropertiessByOperationAutoRequest getTestLocationCollectionPropertiessByOperationAutoRequest)
        {
            SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode = getTestLocationCollectionPropertiessByOperationAutoRequest.securityOperationCode;
            string odataQueryString = getTestLocationCollectionPropertiessByOperationAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestLocationCollectionPropertiessByOperationInternal(odataQueryString, securityOperationCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<SampleSystem.Generated.DTO.TestLocationCollectionPropertiesProjectionDTO> GetTestLocationCollectionPropertiessByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLocationCollectionPropertiesBLL bll = evaluateData.Context.Logics.TestLocationCollectionPropertiesFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestLocationCollectionProperties> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestLocationCollectionProperties>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataList = bll.GetObjectsByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestLocationCollectionProperties>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.Select(odataList, testLocationCollectionProperties => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(testLocationCollectionProperties, evaluateData.MappingService));
        }
        
        /// <summary>
        /// Get hierarchical data of type TestLocationCollectionPropertiess (ProjectionDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestLocationCollectionPropertiesTreeByOperation")]
        public virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestLocationCollectionPropertiesProjectionDTO, System.Guid>> GetTestLocationCollectionPropertiesTreeByOperation(GetTestLocationCollectionPropertiesTreeByOperationAutoRequest getTestLocationCollectionPropertiesTreeByOperationAutoRequest)
        {
            SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode = getTestLocationCollectionPropertiesTreeByOperationAutoRequest.securityOperationCode;
            string odataQueryString = getTestLocationCollectionPropertiesTreeByOperationAutoRequest.odataQueryString;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestLocationCollectionPropertiesTreeByOperationInternal(odataQueryString, securityOperationCode, evaluateData));
        }
        
        protected virtual Framework.OData.SelectOperationResult<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.TestLocationCollectionPropertiesProjectionDTO, System.Guid>> GetTestLocationCollectionPropertiesTreeByOperationInternal(string odataQueryString, SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestLocationCollectionPropertiesBLL bll = evaluateData.Context.Logics.TestLocationCollectionPropertiesFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            Framework.OData.SelectOperation selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
            Framework.OData.SelectOperation<SampleSystem.Domain.Projections.TestLocationCollectionProperties> typedSelectOperation = Framework.OData.StandartExpressionBuilderExtensions.ToTyped<SampleSystem.Domain.Projections.TestLocationCollectionProperties>(evaluateData.Context.StandartExpressionBuilder, selectOperation);
            var odataTree = bll.GetTreeByOData(typedSelectOperation, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestLocationCollectionProperties>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return Framework.OData.SelectOperationResultExtensions.SelectN(odataTree, testLocationCollectionProperties => SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(testLocationCollectionProperties, evaluateData.MappingService));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestLocationsByOperationAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestLocationTreeByOperationAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestLocationCollectionPropertiessByOperationAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestLocationCollectionPropertiesTreeByOperationAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public string odataQueryString;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.Generated.DTO.SampleSystemLocationSecurityOperationCode securityOperationCode;
    }
}
