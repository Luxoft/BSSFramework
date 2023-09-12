namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class ReportPropertyController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        /// <summary>
        /// Check ReportProperty access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckReportPropertyAccess")]
        public virtual void CheckReportPropertyAccess(CheckReportPropertyAccessAutoRequest checkReportPropertyAccessAutoRequest)
        {
            string securityOperationName = checkReportPropertyAccessAutoRequest.securityOperationName;
            Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdent = checkReportPropertyAccessAutoRequest.reportPropertyIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckReportPropertyAccessInternal(reportPropertyIdent, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckReportPropertyAccessInternal(Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportPropertyBLL bll = evaluateData.Context.Logics.ReportProperty;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(securityOperationName, typeof(Framework.Configuration.ConfigurationSecurityOperation));
            Framework.Configuration.Domain.Reports.ReportProperty domainObject = bll.GetById(reportPropertyIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Reports.ReportProperty>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get full list of ReportProperties (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportProperties")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportPropertyFullDTO> GetFullReportProperties()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportPropertiesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ReportProperties (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportPropertiesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportPropertyFullDTO> GetFullReportPropertiesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO[] reportPropertyIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportPropertiesByIdentsInternal(reportPropertyIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportPropertyFullDTO> GetFullReportPropertiesByIdentsInternal(Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO[] reportPropertyIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportPropertyBLL bll = evaluateData.Context.Logics.ReportPropertyFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(reportPropertyIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportProperty>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportPropertyFullDTO> GetFullReportPropertiesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportPropertyBLL bll = evaluateData.Context.Logics.ReportPropertyFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportProperty>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ReportProperty (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportProperty")]
        public virtual Framework.Configuration.Generated.DTO.ReportPropertyFullDTO GetFullReportProperty([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportPropertyInternal(reportPropertyIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportPropertyFullDTO GetFullReportPropertyInternal(Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportPropertyBLL bll = evaluateData.Context.Logics.ReportPropertyFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.ReportProperty domainObject = bll.GetById(reportPropertyIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportProperty>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ReportProperty (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichReportProperty")]
        public virtual Framework.Configuration.Generated.DTO.ReportPropertyRichDTO GetRichReportProperty([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichReportPropertyInternal(reportPropertyIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportPropertyRichDTO GetRichReportPropertyInternal(Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportPropertyBLL bll = evaluateData.Context.Logics.ReportPropertyFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.ReportProperty domainObject = bll.GetById(reportPropertyIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportProperty>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ReportProperties (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportProperties")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportPropertySimpleDTO> GetSimpleReportProperties()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportPropertiesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ReportProperties (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportPropertiesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportPropertySimpleDTO> GetSimpleReportPropertiesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO[] reportPropertyIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportPropertiesByIdentsInternal(reportPropertyIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportPropertySimpleDTO> GetSimpleReportPropertiesByIdentsInternal(Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO[] reportPropertyIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportPropertyBLL bll = evaluateData.Context.Logics.ReportPropertyFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(reportPropertyIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportProperty>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportPropertySimpleDTO> GetSimpleReportPropertiesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportPropertyBLL bll = evaluateData.Context.Logics.ReportPropertyFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportProperty>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ReportProperty (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportProperty")]
        public virtual Framework.Configuration.Generated.DTO.ReportPropertySimpleDTO GetSimpleReportProperty([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportPropertyInternal(reportPropertyIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportPropertySimpleDTO GetSimpleReportPropertyInternal(Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportPropertyBLL bll = evaluateData.Context.Logics.ReportPropertyFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.ReportProperty domainObject = bll.GetById(reportPropertyIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportProperty>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for ReportProperty
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasReportPropertyAccess")]
        public virtual bool HasReportPropertyAccess(HasReportPropertyAccessAutoRequest hasReportPropertyAccessAutoRequest)
        {
            string securityOperationName = hasReportPropertyAccessAutoRequest.securityOperationName;
            Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdent = hasReportPropertyAccessAutoRequest.reportPropertyIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasReportPropertyAccessInternal(reportPropertyIdent, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasReportPropertyAccessInternal(Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportPropertyBLL bll = evaluateData.Context.Logics.ReportProperty;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(securityOperationName, typeof(Framework.Configuration.ConfigurationSecurityOperation));
            Framework.Configuration.Domain.Reports.ReportProperty domainObject = bll.GetById(reportPropertyIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Reports.ReportProperty>(operation).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckReportPropertyAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasReportPropertyAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ReportPropertyIdentityDTO reportPropertyIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
