﻿namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class BusinessUnitHrDepartmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO GetFullBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO GetFullBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetById(businessUnitHrDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitHrDepartments (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO> GetFullBusinessUnitHrDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitHrDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartments (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO> GetFullBusinessUnitHrDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO[] businessUnitHrDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitHrDepartmentsByIdentsInternal(businessUnitHrDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO> GetFullBusinessUnitHrDepartmentsByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO[] businessUnitHrDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(businessUnitHrDepartmentIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO> GetFullBusinessUnitHrDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentRichDTO GetRichBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentRichDTO GetRichBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetById(businessUnitHrDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO GetSimpleBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO GetSimpleBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetById(businessUnitHrDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitHrDepartments (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO> GetSimpleBusinessUnitHrDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitHrDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartments (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO> GetSimpleBusinessUnitHrDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO[] businessUnitHrDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitHrDepartmentsByIdentsInternal(businessUnitHrDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO> GetSimpleBusinessUnitHrDepartmentsByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO[] businessUnitHrDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(businessUnitHrDepartmentIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO> GetSimpleBusinessUnitHrDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitHrDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Remove BusinessUnitHrDepartment
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual void RemoveBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemoveBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentIdent, evaluateData));
        }
        
        protected virtual void RemoveBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
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
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO SaveBusinessUnitHrDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentStrictDTO businessUnitHrDepartmentStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveBusinessUnitHrDepartmentInternal(businessUnitHrDepartmentStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO SaveBusinessUnitHrDepartmentInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentStrictDTO businessUnitHrDepartmentStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
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
}
