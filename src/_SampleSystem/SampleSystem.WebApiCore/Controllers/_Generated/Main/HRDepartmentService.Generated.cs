namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class HRDepartmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get HRDepartment (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentInternal(hRDepartmentIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartment (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentByCodeInternal(hRDepartmentCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentByCodeInternal(string hRDepartmentCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, hRDepartmentCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentByNameInternal(hRDepartmentName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentByNameInternal(string hRDepartmentName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, hRDepartmentName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of HRDepartments (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartments (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentsByIdentsInternal(hRDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsByIdentsInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(hRDepartmentIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentFullDTO> GetFullHRDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichHRDepartmentInternal(hRDepartmentIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartment (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichHRDepartmentByCodeInternal(hRDepartmentCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentByCodeInternal(string hRDepartmentCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, hRDepartmentCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichHRDepartmentByNameInternal(hRDepartmentName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentByNameInternal(string hRDepartmentName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, hRDepartmentName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentInternal(hRDepartmentIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartment (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentByCodeInternal(hRDepartmentCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentByCodeInternal(string hRDepartmentCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, hRDepartmentCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentByNameInternal(hRDepartmentName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentByNameInternal(string hRDepartmentName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, hRDepartmentName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of HRDepartments (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartments (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentsByIdentsInternal(hRDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsByIdentsInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(hRDepartmentIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentSimpleDTO> GetSimpleHRDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentInternal(hRDepartmentIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartment (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentByCodeInternal(hRDepartmentCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentByCodeInternal(string hRDepartmentCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, hRDepartmentCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string hRDepartmentName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentByNameInternal(hRDepartmentName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentByNameInternal(string hRDepartmentName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, hRDepartmentName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of HRDepartments (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartments()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get HRDepartments (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentsByIdentsInternal(hRDepartmentIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsByIdentsInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO[] hRDepartmentIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(hRDepartmentIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.HRDepartmentVisualDTO> GetVisualHRDepartmentsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.HRDepartment>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Remove HRDepartment
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual void RemoveHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemoveHRDepartmentInternal(hRDepartmentIdent, evaluateData));
        }
        
        protected virtual void RemoveHRDepartmentInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.Edit);
            this.RemoveHRDepartmentInternal(hRDepartmentIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveHRDepartmentInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IHRDepartmentBLL bll)
        {
            SampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save HRDepartments
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.HRDepartmentIdentityDTO SaveHRDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentStrictDTO hRDepartmentStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveHRDepartmentInternal(hRDepartmentStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentIdentityDTO SaveHRDepartmentInternal(SampleSystem.Generated.DTO.HRDepartmentStrictDTO hRDepartmentStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(SecuritySystem.SecurityRule.Edit);
            return this.SaveHRDepartmentInternal(hRDepartmentStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentIdentityDTO SaveHRDepartmentInternal(SampleSystem.Generated.DTO.HRDepartmentStrictDTO hRDepartmentStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IHRDepartmentBLL bll)
        {
            SampleSystem.Domain.HRDepartment domainObject = bll.GetById(hRDepartmentStrict.Id, true);
            hRDepartmentStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        /// <summary>
        /// Get TestDepartment (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestDepartmentProjectionDTO GetTestDepartment([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO testDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestDepartmentInternal(testDepartmentIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestDepartmentProjectionDTO GetTestDepartmentInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO testDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestDepartmentBLL bll = evaluateData.Context.Logics.TestDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Projections.TestDepartment domainObject = bll.GetById(testDepartmentIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.TestDepartment>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
    }
}
