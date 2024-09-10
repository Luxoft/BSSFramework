namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class ManagementUnitAndBusinessUnitLinkController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO GetFullManagementUnitAndBusinessUnitLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndBusinessUnitLinkInternal(managementUnitAndBusinessUnitLinkIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO GetFullManagementUnitAndBusinessUnitLinkByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitAndBusinessUnitLinkName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndBusinessUnitLinkByNameInternal(managementUnitAndBusinessUnitLinkName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO GetFullManagementUnitAndBusinessUnitLinkByNameInternal(string managementUnitAndBusinessUnitLinkName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitAndBusinessUnitLinkName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO GetFullManagementUnitAndBusinessUnitLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetById(managementUnitAndBusinessUnitLinkIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ManagementUnitAndBusinessUnitLinks (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO> GetFullManagementUnitAndBusinessUnitLinks()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndBusinessUnitLinksInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLinks (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO> GetFullManagementUnitAndBusinessUnitLinksByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO[] managementUnitAndBusinessUnitLinkIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndBusinessUnitLinksByIdentsInternal(managementUnitAndBusinessUnitLinkIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO> GetFullManagementUnitAndBusinessUnitLinksByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO[] managementUnitAndBusinessUnitLinkIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(managementUnitAndBusinessUnitLinkIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO> GetFullManagementUnitAndBusinessUnitLinksInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkRichDTO GetRichManagementUnitAndBusinessUnitLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitAndBusinessUnitLinkInternal(managementUnitAndBusinessUnitLinkIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkRichDTO GetRichManagementUnitAndBusinessUnitLinkByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitAndBusinessUnitLinkName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitAndBusinessUnitLinkByNameInternal(managementUnitAndBusinessUnitLinkName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkRichDTO GetRichManagementUnitAndBusinessUnitLinkByNameInternal(string managementUnitAndBusinessUnitLinkName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitAndBusinessUnitLinkName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkRichDTO GetRichManagementUnitAndBusinessUnitLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetById(managementUnitAndBusinessUnitLinkIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO GetSimpleManagementUnitAndBusinessUnitLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndBusinessUnitLinkInternal(managementUnitAndBusinessUnitLinkIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO GetSimpleManagementUnitAndBusinessUnitLinkByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitAndBusinessUnitLinkName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndBusinessUnitLinkByNameInternal(managementUnitAndBusinessUnitLinkName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO GetSimpleManagementUnitAndBusinessUnitLinkByNameInternal(string managementUnitAndBusinessUnitLinkName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitAndBusinessUnitLinkName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO GetSimpleManagementUnitAndBusinessUnitLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetById(managementUnitAndBusinessUnitLinkIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ManagementUnitAndBusinessUnitLinks (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO> GetSimpleManagementUnitAndBusinessUnitLinks()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndBusinessUnitLinksInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLinks (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO> GetSimpleManagementUnitAndBusinessUnitLinksByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO[] managementUnitAndBusinessUnitLinkIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndBusinessUnitLinksByIdentsInternal(managementUnitAndBusinessUnitLinkIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO> GetSimpleManagementUnitAndBusinessUnitLinksByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO[] managementUnitAndBusinessUnitLinkIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(managementUnitAndBusinessUnitLinkIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO> GetSimpleManagementUnitAndBusinessUnitLinksInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Remove ManagementUnitAndBusinessUnitLink
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual void RemoveManagementUnitAndBusinessUnitLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemoveManagementUnitAndBusinessUnitLinkInternal(managementUnitAndBusinessUnitLinkIdent, evaluateData));
        }
        
        protected virtual void RemoveManagementUnitAndBusinessUnitLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
            this.RemoveManagementUnitAndBusinessUnitLinkInternal(managementUnitAndBusinessUnitLinkIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveManagementUnitAndBusinessUnitLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll)
        {
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetById(managementUnitAndBusinessUnitLinkIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save ManagementUnitAndBusinessUnitLinks
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO SaveManagementUnitAndBusinessUnitLink([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkStrictDTO managementUnitAndBusinessUnitLinkStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveManagementUnitAndBusinessUnitLinkInternal(managementUnitAndBusinessUnitLinkStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO SaveManagementUnitAndBusinessUnitLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkStrictDTO managementUnitAndBusinessUnitLinkStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
            return this.SaveManagementUnitAndBusinessUnitLinkInternal(managementUnitAndBusinessUnitLinkStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO SaveManagementUnitAndBusinessUnitLinkInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkStrictDTO managementUnitAndBusinessUnitLinkStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll)
        {
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, managementUnitAndBusinessUnitLinkStrict.Id);
            managementUnitAndBusinessUnitLinkStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
