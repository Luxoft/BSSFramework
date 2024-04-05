namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class ManagementUnitController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get ManagementUnit (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnit")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitInternal(managementUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnit (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitByName")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitByNameInternal(managementUnitName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitByNameInternal(string managementUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetById(managementUnitIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ManagementUnits (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnits")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnits()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnits (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitsByIdentsInternal(managementUnitIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(managementUnitIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichManagementUnit")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitInternal(managementUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnit (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichManagementUnitByName")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitByNameInternal(managementUnitName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitByNameInternal(string managementUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetById(managementUnitIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnit")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitInternal(managementUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnit (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitByName")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitByNameInternal(managementUnitName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitByNameInternal(string managementUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetById(managementUnitIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ManagementUnits (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnits")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnits()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnits (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitsByIdentsInternal(managementUnitIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(managementUnitIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualManagementUnit")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitInternal(managementUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnit (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualManagementUnitByName")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitByNameInternal(managementUnitName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitByNameInternal(string managementUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetById(managementUnitIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ManagementUnits (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualManagementUnits")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnits()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnits (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualManagementUnitsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitsByIdentsInternal(managementUnitIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(managementUnitIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save ManagementUnits
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveManagementUnit")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO SaveManagementUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitStrictDTO managementUnitStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveManagementUnitInternal(managementUnitStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO SaveManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitStrictDTO managementUnitStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
            return this.SaveManagementUnitInternal(managementUnitStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO SaveManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitStrictDTO managementUnitStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IManagementUnitBLL bll)
        {
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetById(managementUnitStrict.Id, true);
            managementUnitStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
