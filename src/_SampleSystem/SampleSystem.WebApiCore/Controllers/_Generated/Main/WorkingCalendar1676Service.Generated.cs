﻿namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]")]
    public partial class WorkingCalendar1676Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get WorkingCalendar1676 (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkingCalendar1676")]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullWorkingCalendar1676Internal(workingCalendar1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkingCalendar1676ByName")]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workingCalendar1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullWorkingCalendar1676ByNameInternal(workingCalendar1676Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676ByNameInternal(string workingCalendar1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workingCalendar1676Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676Internal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkingCalendar1676s (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkingCalendar1676s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO> GetFullWorkingCalendar1676s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullWorkingCalendar1676sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676s (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkingCalendar1676sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO> GetFullWorkingCalendar1676sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullWorkingCalendar1676sByIdentsInternal(workingCalendar1676Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO> GetFullWorkingCalendar1676sByIdentsInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(workingCalendar1676Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO> GetFullWorkingCalendar1676sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichWorkingCalendar1676")]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichWorkingCalendar1676Internal(workingCalendar1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichWorkingCalendar1676ByName")]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workingCalendar1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichWorkingCalendar1676ByNameInternal(workingCalendar1676Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676ByNameInternal(string workingCalendar1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workingCalendar1676Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676Internal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkingCalendar1676")]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleWorkingCalendar1676Internal(workingCalendar1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkingCalendar1676ByName")]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workingCalendar1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleWorkingCalendar1676ByNameInternal(workingCalendar1676Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676ByNameInternal(string workingCalendar1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workingCalendar1676Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676Internal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkingCalendar1676s (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkingCalendar1676s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO> GetSimpleWorkingCalendar1676s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleWorkingCalendar1676sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676s (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkingCalendar1676sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO> GetSimpleWorkingCalendar1676sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleWorkingCalendar1676sByIdentsInternal(workingCalendar1676Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO> GetSimpleWorkingCalendar1676sByIdentsInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(workingCalendar1676Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO> GetSimpleWorkingCalendar1676sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkingCalendar1676")]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualWorkingCalendar1676Internal(workingCalendar1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkingCalendar1676ByName")]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workingCalendar1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualWorkingCalendar1676ByNameInternal(workingCalendar1676Name, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676ByNameInternal(string workingCalendar1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workingCalendar1676Name, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676Internal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkingCalendar1676s (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkingCalendar1676s")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO> GetVisualWorkingCalendar1676s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualWorkingCalendar1676sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676s (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkingCalendar1676sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO> GetVisualWorkingCalendar1676sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualWorkingCalendar1676sByIdentsInternal(workingCalendar1676Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO> GetVisualWorkingCalendar1676sByIdentsInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(workingCalendar1676Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO> GetVisualWorkingCalendar1676sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
