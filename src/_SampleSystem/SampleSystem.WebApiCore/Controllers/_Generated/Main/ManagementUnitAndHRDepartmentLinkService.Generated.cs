namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class ManagementUnitAndHRDepartmentLinkController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check ManagementUnitAndHRDepartmentLink access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckManagementUnitAndHRDepartmentLinkAccess")]
        public virtual void CheckManagementUnitAndHRDepartmentLinkAccess(CheckManagementUnitAndHRDepartmentLinkAccessAutoRequest checkManagementUnitAndHRDepartmentLinkAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkManagementUnitAndHRDepartmentLinkAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent = checkManagementUnitAndHRDepartmentLinkAccessAutoRequest.managementUnitAndHRDepartmentLinkIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckManagementUnitAndHRDepartmentLinkAccessInternal(managementUnitAndHRDepartmentLinkIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckManagementUnitAndHRDepartmentLinkAccessInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLink;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitAndHRDepartmentLink")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ManagementUnitAndHRDepartmentLinks (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitAndHRDepartmentLinks")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinks()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinksInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLinks (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitAndHRDepartmentLinksByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO[] managementUnitAndHRDepartmentLinkIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinksByIdentsInternal(managementUnitAndHRDepartmentLinkIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO[] managementUnitAndHRDepartmentLinkIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(managementUnitAndHRDepartmentLinkIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO> GetFullManagementUnitAndHRDepartmentLinksInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichManagementUnitAndHRDepartmentLink")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitAndHRDepartmentLink")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ManagementUnitAndHRDepartmentLinks (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitAndHRDepartmentLinks")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinks()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinksInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLinks (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitAndHRDepartmentLinksByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO[] managementUnitAndHRDepartmentLinkIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinksByIdentsInternal(managementUnitAndHRDepartmentLinkIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO[] managementUnitAndHRDepartmentLinkIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(managementUnitAndHRDepartmentLinkIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO> GetSimpleManagementUnitAndHRDepartmentLinksInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for ManagementUnitAndHRDepartmentLink
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasManagementUnitAndHRDepartmentLinkAccess")]
        public virtual bool HasManagementUnitAndHRDepartmentLinkAccess(HasManagementUnitAndHRDepartmentLinkAccessAutoRequest hasManagementUnitAndHRDepartmentLinkAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasManagementUnitAndHRDepartmentLinkAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent = hasManagementUnitAndHRDepartmentLinkAccessAutoRequest.managementUnitAndHRDepartmentLinkIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasManagementUnitAndHRDepartmentLinkAccessInternal(managementUnitAndHRDepartmentLinkIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasManagementUnitAndHRDepartmentLinkAccessInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLink;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove ManagementUnitAndHRDepartmentLink
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveManagementUnitAndHRDepartmentLink")]
        public virtual void RemoveManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdent, evaluateData));
        }
        
        protected virtual void RemoveManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll)
        {
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetById(managementUnitAndHRDepartmentLinkIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save ManagementUnitAndHRDepartmentLinks
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveManagementUnitAndHRDepartmentLink")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO SaveManagementUnitAndHRDepartmentLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkStrictDTO managementUnitAndHRDepartmentLinkStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO SaveManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkStrictDTO managementUnitAndHRDepartmentLinkStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveManagementUnitAndHRDepartmentLinkInternal(managementUnitAndHRDepartmentLinkStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO SaveManagementUnitAndHRDepartmentLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkStrictDTO managementUnitAndHRDepartmentLinkStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll)
        {
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, managementUnitAndHRDepartmentLinkStrict.Id);
            managementUnitAndHRDepartmentLinkStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckManagementUnitAndHRDepartmentLinkAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasManagementUnitAndHRDepartmentLinkAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
