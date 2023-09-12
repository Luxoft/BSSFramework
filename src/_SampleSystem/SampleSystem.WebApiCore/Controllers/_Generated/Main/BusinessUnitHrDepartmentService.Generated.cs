namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitHrDepartmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check BusinessUnitHrDepartment access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckBusinessUnitHrDepartmentAccess")]
        public virtual void CheckBusinessUnitHrDepartmentAccess(CheckBusinessUnitHrDepartmentAccessAutoRequest checkBusinessUnitHrDepartmentAccessAutoRequest)
        {
            string securityOperationName = checkBusinessUnitHrDepartmentAccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent = checkBusinessUnitHrDepartmentAccessAutoRequest.businessUnitHrDepartmentIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckBusinessUnitHrDepartmentAccessInternal(businessUnitHrDepartmentIdent, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckBusinessUnitHrDepartmentAccessInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartment;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(securityOperationName, typeof(SampleSystem.SampleSystemSecurityOperation));
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetById(businessUnitHrDepartmentIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.BusinessUnitHrDepartment>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitHrDepartment")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO GetFullBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO GetFullBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetById(businessUnitHrDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitHrDepartments (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitHrDepartments")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO> GetFullBusinessUnitHrDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitHrDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartments (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitHrDepartmentsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO> GetFullBusinessUnitHrDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO[] businessUnitHrDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitHrDepartmentsByIdentsInternal(businessUnitHrDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO> GetFullBusinessUnitHrDepartmentsByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO[] businessUnitHrDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(businessUnitHrDepartmentIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO> GetFullBusinessUnitHrDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessUnitHrDepartment")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentRichDTO GetRichBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentRichDTO GetRichBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetById(businessUnitHrDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitHrDepartment")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO GetSimpleBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO GetSimpleBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetById(businessUnitHrDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitHrDepartments (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitHrDepartments")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO> GetSimpleBusinessUnitHrDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitHrDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartments (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitHrDepartmentsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO> GetSimpleBusinessUnitHrDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO[] businessUnitHrDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitHrDepartmentsByIdentsInternal(businessUnitHrDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO> GetSimpleBusinessUnitHrDepartmentsByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO[] businessUnitHrDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(businessUnitHrDepartmentIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO> GetSimpleBusinessUnitHrDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for BusinessUnitHrDepartment
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasBusinessUnitHrDepartmentAccess")]
        public virtual bool HasBusinessUnitHrDepartmentAccess(HasBusinessUnitHrDepartmentAccessAutoRequest hasBusinessUnitHrDepartmentAccessAutoRequest)
        {
            string securityOperationName = hasBusinessUnitHrDepartmentAccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent = hasBusinessUnitHrDepartmentAccessAutoRequest.businessUnitHrDepartmentIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasBusinessUnitHrDepartmentAccessInternal(businessUnitHrDepartmentIdent, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasBusinessUnitHrDepartmentAccessInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartment;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(securityOperationName, typeof(SampleSystem.SampleSystemSecurityOperation));
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetById(businessUnitHrDepartmentIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.BusinessUnitHrDepartment>(operation).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove BusinessUnitHrDepartment
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveBusinessUnitHrDepartment")]
        public virtual void RemoveBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemoveBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentIdent, evaluateData));
        }
        
        protected virtual void RemoveBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll)
        {
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetById(businessUnitHrDepartmentIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save BusinessUnitHrDepartments
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveBusinessUnitHrDepartment")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO SaveBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentStrictDTO businessUnitHrDepartmentStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO SaveBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentStrictDTO businessUnitHrDepartmentStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO SaveBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentStrictDTO businessUnitHrDepartmentStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll)
        {
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, businessUnitHrDepartmentStrict.Id);
            businessUnitHrDepartmentStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckBusinessUnitHrDepartmentAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasBusinessUnitHrDepartmentAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
