namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class CompanyLegalEntityController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get full list of CompanyLegalEntities (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntities")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntities()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntitiesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntitiesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntitiesByIdentsInternal(companyLegalEntityIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesByIdentsInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(companyLegalEntityIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntity")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntityInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntityByCode")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntityByCodeInternal(companyLegalEntityCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityByCodeInternal(string companyLegalEntityCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, companyLegalEntityCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntityByName")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntityByNameInternal(companyLegalEntityName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityByNameInternal(string companyLegalEntityName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, companyLegalEntityName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCompanyLegalEntity")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichCompanyLegalEntityInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCompanyLegalEntityByCode")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichCompanyLegalEntityByCodeInternal(companyLegalEntityCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityByCodeInternal(string companyLegalEntityCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, companyLegalEntityCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCompanyLegalEntityByName")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichCompanyLegalEntityByNameInternal(companyLegalEntityName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityByNameInternal(string companyLegalEntityName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, companyLegalEntityName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of CompanyLegalEntities (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntities")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntities()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntitiesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntitiesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntitiesByIdentsInternal(companyLegalEntityIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesByIdentsInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(companyLegalEntityIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntity")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntityInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntityByCode")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntityByCodeInternal(companyLegalEntityCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityByCodeInternal(string companyLegalEntityCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, companyLegalEntityCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntityByName")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntityByNameInternal(companyLegalEntityName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityByNameInternal(string companyLegalEntityName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, companyLegalEntityName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of CompanyLegalEntities (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntities")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntities()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntitiesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntitiesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntitiesByIdentsInternal(companyLegalEntityIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesByIdentsInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(companyLegalEntityIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntity")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntityInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntityByCode")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntityByCodeInternal(companyLegalEntityCode, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityByCodeInternal(string companyLegalEntityCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, companyLegalEntityCode, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntityByName")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntityByNameInternal(companyLegalEntityName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityByNameInternal(string companyLegalEntityName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, companyLegalEntityName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Remove CompanyLegalEntity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveCompanyLegalEntity")]
        public virtual void RemoveCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemoveCompanyLegalEntityInternal(companyLegalEntityIdent, evaluateData));
        }
        
        protected virtual void RemoveCompanyLegalEntityInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveCompanyLegalEntityInternal(companyLegalEntityIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveCompanyLegalEntityInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ICompanyLegalEntityBLL bll)
        {
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save CompanyLegalEntities
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveCompanyLegalEntity")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO SaveCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityStrictDTO companyLegalEntityStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveCompanyLegalEntityInternal(companyLegalEntityStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO SaveCompanyLegalEntityInternal(SampleSystem.Generated.DTO.CompanyLegalEntityStrictDTO companyLegalEntityStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveCompanyLegalEntityInternal(companyLegalEntityStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO SaveCompanyLegalEntityInternal(SampleSystem.Generated.DTO.CompanyLegalEntityStrictDTO companyLegalEntityStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ICompanyLegalEntityBLL bll)
        {
            SampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, companyLegalEntityStrict.Id);
            companyLegalEntityStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        /// <summary>
        /// Get CustomCompanyLegalEntity (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCustomCompanyLegalEntity")]
        public virtual SampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO GetCustomCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO customCompanyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCustomCompanyLegalEntityInternal(customCompanyLegalEntityIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO GetCustomCompanyLegalEntityInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO customCompanyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICustomCompanyLegalEntityBLL bll = evaluateData.Context.Logics.CustomCompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.Projections.CustomCompanyLegalEntity domainObject = bll.GetById(customCompanyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Projections.CustomCompanyLegalEntity>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
    }
}
