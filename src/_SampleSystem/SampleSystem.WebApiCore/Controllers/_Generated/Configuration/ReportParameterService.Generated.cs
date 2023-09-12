namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class ReportParameterController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        /// <summary>
        /// Check ReportParameter access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckReportParameterAccess")]
        public virtual void CheckReportParameterAccess(CheckReportParameterAccessAutoRequest checkReportParameterAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkReportParameterAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdent = checkReportParameterAccessAutoRequest.reportParameterIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckReportParameterAccessInternal(reportParameterIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckReportParameterAccessInternal(Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportParameterBLL bll = evaluateData.Context.Logics.ReportParameter;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            Framework.Configuration.Domain.Reports.ReportParameter domainObject = bll.GetById(reportParameterIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Reports.ReportParameter>(securityOperationCode), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get ReportParameter (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportParameter")]
        public virtual Framework.Configuration.Generated.DTO.ReportParameterFullDTO GetFullReportParameter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportParameterInternal(reportParameterIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportParameterFullDTO GetFullReportParameterInternal(Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportParameterBLL bll = evaluateData.Context.Logics.ReportParameterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.ReportParameter domainObject = bll.GetById(reportParameterIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportParameter>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ReportParameters (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportParameters")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportParameterFullDTO> GetFullReportParameters()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportParametersInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ReportParameters (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportParametersByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportParameterFullDTO> GetFullReportParametersByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO[] reportParameterIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportParametersByIdentsInternal(reportParameterIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportParameterFullDTO> GetFullReportParametersByIdentsInternal(Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO[] reportParameterIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportParameterBLL bll = evaluateData.Context.Logics.ReportParameterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(reportParameterIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportParameter>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportParameterFullDTO> GetFullReportParametersInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportParameterBLL bll = evaluateData.Context.Logics.ReportParameterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportParameter>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ReportParameter (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichReportParameter")]
        public virtual Framework.Configuration.Generated.DTO.ReportParameterRichDTO GetRichReportParameter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichReportParameterInternal(reportParameterIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportParameterRichDTO GetRichReportParameterInternal(Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportParameterBLL bll = evaluateData.Context.Logics.ReportParameterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.ReportParameter domainObject = bll.GetById(reportParameterIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportParameter>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ReportParameter (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportParameter")]
        public virtual Framework.Configuration.Generated.DTO.ReportParameterSimpleDTO GetSimpleReportParameter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportParameterInternal(reportParameterIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportParameterSimpleDTO GetSimpleReportParameterInternal(Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportParameterBLL bll = evaluateData.Context.Logics.ReportParameterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.ReportParameter domainObject = bll.GetById(reportParameterIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportParameter>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ReportParameters (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportParameters")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportParameterSimpleDTO> GetSimpleReportParameters()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportParametersInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ReportParameters (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportParametersByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportParameterSimpleDTO> GetSimpleReportParametersByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO[] reportParameterIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportParametersByIdentsInternal(reportParameterIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportParameterSimpleDTO> GetSimpleReportParametersByIdentsInternal(Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO[] reportParameterIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportParameterBLL bll = evaluateData.Context.Logics.ReportParameterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(reportParameterIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportParameter>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportParameterSimpleDTO> GetSimpleReportParametersInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportParameterBLL bll = evaluateData.Context.Logics.ReportParameterFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.ReportParameter>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for ReportParameter
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasReportParameterAccess")]
        public virtual bool HasReportParameterAccess(HasReportParameterAccessAutoRequest hasReportParameterAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasReportParameterAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdent = hasReportParameterAccessAutoRequest.reportParameterIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasReportParameterAccessInternal(reportParameterIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasReportParameterAccessInternal(Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportParameterBLL bll = evaluateData.Context.Logics.ReportParameter;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            Framework.Configuration.Domain.Reports.ReportParameter domainObject = bll.GetById(reportParameterIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Reports.ReportParameter>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckReportParameterAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasReportParameterAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ReportParameterIdentityDTO reportParameterIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
