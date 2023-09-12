namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class ReportFilterController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        /// <summary>
        /// Check ReportFilter access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckReportFilterAccess")]
        public virtual void CheckReportFilterAccess(CheckReportFilterAccessAutoRequest checkReportFilterAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkReportFilterAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdent = checkReportFilterAccessAutoRequest.reportFilterIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckReportFilterAccessInternal(reportFilterIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckReportFilterAccessInternal(Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportFilterBLL bll = evaluateData.Context.Logics.ReportFilter;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            Framework.Configuration.Domain.Reports.ReportFilter domainObject = bll.GetById(reportFilterIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Reports.ReportFilter>(securityOperationCode), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get ReportFilter (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportFilter")]
        public virtual Framework.Configuration.Generated.DTO.ReportFilterFullDTO GetFullReportFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportFilterInternal(reportFilterIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportFilterFullDTO GetFullReportFilterInternal(Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportFilterBLL bll = evaluateData.Context.Logics.ReportFilterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.ReportFilter domainObject = bll.GetById(reportFilterIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportFilter>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ReportFilters (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportFilters")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFilterFullDTO> GetFullReportFilters()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportFiltersInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ReportFilters (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportFiltersByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFilterFullDTO> GetFullReportFiltersByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO[] reportFilterIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportFiltersByIdentsInternal(reportFilterIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFilterFullDTO> GetFullReportFiltersByIdentsInternal(Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO[] reportFilterIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportFilterBLL bll = evaluateData.Context.Logics.ReportFilterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(reportFilterIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportFilter>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFilterFullDTO> GetFullReportFiltersInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportFilterBLL bll = evaluateData.Context.Logics.ReportFilterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportFilter>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ReportFilter (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichReportFilter")]
        public virtual Framework.Configuration.Generated.DTO.ReportFilterRichDTO GetRichReportFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichReportFilterInternal(reportFilterIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportFilterRichDTO GetRichReportFilterInternal(Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportFilterBLL bll = evaluateData.Context.Logics.ReportFilterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.ReportFilter domainObject = bll.GetById(reportFilterIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportFilter>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ReportFilter (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportFilter")]
        public virtual Framework.Configuration.Generated.DTO.ReportFilterSimpleDTO GetSimpleReportFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportFilterInternal(reportFilterIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportFilterSimpleDTO GetSimpleReportFilterInternal(Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportFilterBLL bll = evaluateData.Context.Logics.ReportFilterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.ReportFilter domainObject = bll.GetById(reportFilterIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportFilter>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ReportFilters (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportFilters")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFilterSimpleDTO> GetSimpleReportFilters()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportFiltersInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ReportFilters (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportFiltersByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFilterSimpleDTO> GetSimpleReportFiltersByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO[] reportFilterIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportFiltersByIdentsInternal(reportFilterIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFilterSimpleDTO> GetSimpleReportFiltersByIdentsInternal(Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO[] reportFilterIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportFilterBLL bll = evaluateData.Context.Logics.ReportFilterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(reportFilterIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportFilter>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFilterSimpleDTO> GetSimpleReportFiltersInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportFilterBLL bll = evaluateData.Context.Logics.ReportFilterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportFilter>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for ReportFilter
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasReportFilterAccess")]
        public virtual bool HasReportFilterAccess(HasReportFilterAccessAutoRequest hasReportFilterAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasReportFilterAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdent = hasReportFilterAccessAutoRequest.reportFilterIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasReportFilterAccessInternal(reportFilterIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasReportFilterAccessInternal(Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportFilterBLL bll = evaluateData.Context.Logics.ReportFilter;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            Framework.Configuration.Domain.Reports.ReportFilter domainObject = bll.GetById(reportFilterIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Reports.ReportFilter>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckReportFilterAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasReportFilterAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ReportFilterIdentityDTO reportFilterIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
