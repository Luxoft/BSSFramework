namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]")]
    public partial class EmployeePositionController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeePosition (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeePosition")]
        public virtual SampleSystem.Generated.DTO.EmployeePositionFullDTO GetFullEmployeePosition([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePositionInternal(employeePositionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePosition (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeePositionByName")]
        public virtual SampleSystem.Generated.DTO.EmployeePositionFullDTO GetFullEmployeePositionByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeePositionName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePositionByNameInternal(employeePositionName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionFullDTO GetFullEmployeePositionByNameInternal(string employeePositionName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeePositionName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionFullDTO GetFullEmployeePositionInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = bll.GetById(employeePositionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeePositions (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeePositions")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionFullDTO> GetFullEmployeePositions()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePositionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePositions (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeePositionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionFullDTO> GetFullEmployeePositionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePositionIdentityDTO[] employeePositionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePositionsByIdentsInternal(employeePositionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionFullDTO> GetFullEmployeePositionsByIdentsInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO[] employeePositionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeePositionIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionFullDTO> GetFullEmployeePositionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePosition (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeePosition")]
        public virtual SampleSystem.Generated.DTO.EmployeePositionRichDTO GetRichEmployeePosition([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeePositionInternal(employeePositionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePosition (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeePositionByName")]
        public virtual SampleSystem.Generated.DTO.EmployeePositionRichDTO GetRichEmployeePositionByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeePositionName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeePositionByNameInternal(employeePositionName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionRichDTO GetRichEmployeePositionByNameInternal(string employeePositionName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeePositionName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionRichDTO GetRichEmployeePositionInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = bll.GetById(employeePositionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePosition (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeePosition")]
        public virtual SampleSystem.Generated.DTO.EmployeePositionSimpleDTO GetSimpleEmployeePosition([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePositionInternal(employeePositionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePosition (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeePositionByName")]
        public virtual SampleSystem.Generated.DTO.EmployeePositionSimpleDTO GetSimpleEmployeePositionByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeePositionName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePositionByNameInternal(employeePositionName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionSimpleDTO GetSimpleEmployeePositionByNameInternal(string employeePositionName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeePositionName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionSimpleDTO GetSimpleEmployeePositionInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = bll.GetById(employeePositionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeePositions (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeePositions")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionSimpleDTO> GetSimpleEmployeePositions()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePositionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePositions (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeePositionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionSimpleDTO> GetSimpleEmployeePositionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePositionIdentityDTO[] employeePositionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePositionsByIdentsInternal(employeePositionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionSimpleDTO> GetSimpleEmployeePositionsByIdentsInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO[] employeePositionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeePositionIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionSimpleDTO> GetSimpleEmployeePositionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePosition (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeePosition")]
        public virtual SampleSystem.Generated.DTO.EmployeePositionVisualDTO GetVisualEmployeePosition([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeePositionInternal(employeePositionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePosition (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeePositionByName")]
        public virtual SampleSystem.Generated.DTO.EmployeePositionVisualDTO GetVisualEmployeePositionByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeePositionName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeePositionByNameInternal(employeePositionName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionVisualDTO GetVisualEmployeePositionByNameInternal(string employeePositionName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeePositionName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionVisualDTO GetVisualEmployeePositionInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = bll.GetById(employeePositionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeePositions (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeePositions")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionVisualDTO> GetVisualEmployeePositions()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeePositionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePositions (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeePositionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionVisualDTO> GetVisualEmployeePositionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePositionIdentityDTO[] employeePositionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeePositionsByIdentsInternal(employeePositionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionVisualDTO> GetVisualEmployeePositionsByIdentsInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO[] employeePositionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(employeePositionIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePositionVisualDTO> GetVisualEmployeePositionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePosition>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
