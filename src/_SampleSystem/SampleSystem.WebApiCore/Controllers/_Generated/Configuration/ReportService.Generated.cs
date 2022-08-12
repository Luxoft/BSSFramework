namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class ReportController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        /// <summary>
        /// Check Report access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckReportAccess")]
        public virtual void CheckReportAccess(CheckReportAccessAutoRequest checkReportAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkReportAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdent = checkReportAccessAutoRequest.reportIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckReportAccessInternal(reportIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckReportAccessInternal(Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.Report;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.Reports.Report domainObject = bll.GetById(reportIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Reports.Report>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Get Report (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReport")]
        public virtual Framework.Configuration.Generated.DTO.ReportFullDTO GetFullReport([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportInternal(reportIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportFullDTO GetFullReportInternal(Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.ReportFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.Report domainObject = bll.GetById(reportIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.Report>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Reports (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReports")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFullDTO> GetFullReports()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Reports (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullReportsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFullDTO> GetFullReportsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportIdentityDTO[] reportIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullReportsByIdentsInternal(reportIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFullDTO> GetFullReportsByIdentsInternal(Framework.Configuration.Generated.DTO.ReportIdentityDTO[] reportIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.ReportFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(reportIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.Report>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportFullDTO> GetFullReportsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.ReportFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.Report>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Report (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichReport")]
        public virtual Framework.Configuration.Generated.DTO.ReportRichDTO GetRichReport([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichReportInternal(reportIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportRichDTO GetRichReportInternal(Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.ReportFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.Report domainObject = bll.GetById(reportIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.Report>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Report (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReport")]
        public virtual Framework.Configuration.Generated.DTO.ReportSimpleDTO GetSimpleReport([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportInternal(reportIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportSimpleDTO GetSimpleReportInternal(Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.ReportFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Reports.Report domainObject = bll.GetById(reportIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.Report>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Reports (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReports")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportSimpleDTO> GetSimpleReports()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Reports (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleReportsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportSimpleDTO> GetSimpleReportsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportIdentityDTO[] reportIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleReportsByIdentsInternal(reportIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportSimpleDTO> GetSimpleReportsByIdentsInternal(Framework.Configuration.Generated.DTO.ReportIdentityDTO[] reportIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.ReportFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(reportIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.Report>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ReportSimpleDTO> GetSimpleReportsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.ReportFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Reports.Report>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Report
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasReportAccess")]
        public virtual bool HasReportAccess(HasReportAccessAutoRequest hasReportAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasReportAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdent = hasReportAccessAutoRequest.reportIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasReportAccessInternal(reportIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasReportAccessInternal(Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.Report;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.Reports.Report domainObject = bll.GetById(reportIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Reports.Report>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove Report
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveReport")]
        public virtual void RemoveReport([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemoveReportInternal(reportIdent, evaluateData));
        }
        
        protected virtual void RemoveReportInternal(Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.ReportFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveReportInternal(reportIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveReportInternal(Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.IReportBLL bll)
        {
            Framework.Configuration.Domain.Reports.Report domainObject = bll.GetById(reportIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save Reports
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveReport")]
        public virtual Framework.Configuration.Generated.DTO.ReportIdentityDTO SaveReport([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ReportStrictDTO reportStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveReportInternal(reportStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportIdentityDTO SaveReportInternal(Framework.Configuration.Generated.DTO.ReportStrictDTO reportStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IReportBLL bll = evaluateData.Context.Logics.ReportFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveReportInternal(reportStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ReportIdentityDTO SaveReportInternal(Framework.Configuration.Generated.DTO.ReportStrictDTO reportStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.IReportBLL bll)
        {
            Framework.Configuration.Domain.Reports.Report domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, reportStrict.Id);
            reportStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckReportAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasReportAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ReportIdentityDTO reportIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
