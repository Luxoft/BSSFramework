namespace WorkflowSampleSystem.WebApiCore.Controllers.Main
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class CompanyLegalEntityController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public CompanyLegalEntityController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check CompanyLegalEntity access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckCompanyLegalEntityAccess")]
        public virtual void CheckCompanyLegalEntityAccess(CheckCompanyLegalEntityAccessAutoRequest checkCompanyLegalEntityAccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = checkCompanyLegalEntityAccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent = checkCompanyLegalEntityAccessAutoRequest.companyLegalEntityIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckCompanyLegalEntityAccessInternal(companyLegalEntityIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckCompanyLegalEntityAccessInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntity;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.CompanyLegalEntity>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get full list of CompanyLegalEntities (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntities")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntities()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntitiesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntitiesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntitiesByIdentsInternal(companyLegalEntityIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesByIdentsInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(companyLegalEntityIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO> GetFullCompanyLegalEntitiesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntity")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntityInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntityByCode")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntityByCodeInternal(companyLegalEntityCode, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityByCodeInternal(string companyLegalEntityCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, companyLegalEntityCode, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntityByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntityByNameInternal(companyLegalEntityName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityByNameInternal(string companyLegalEntityName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, companyLegalEntityName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCompanyLegalEntity")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichCompanyLegalEntityInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCompanyLegalEntityByCode")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichCompanyLegalEntityByCodeInternal(companyLegalEntityCode, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityByCodeInternal(string companyLegalEntityCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, companyLegalEntityCode, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCompanyLegalEntityByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichCompanyLegalEntityByNameInternal(companyLegalEntityName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityByNameInternal(string companyLegalEntityName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, companyLegalEntityName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of CompanyLegalEntities (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntities")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntities()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntitiesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntitiesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntitiesByIdentsInternal(companyLegalEntityIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesByIdentsInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(companyLegalEntityIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO> GetSimpleCompanyLegalEntitiesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntity")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntityInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntityByCode")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntityByCodeInternal(companyLegalEntityCode, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityByCodeInternal(string companyLegalEntityCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, companyLegalEntityCode, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntityByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntityByNameInternal(companyLegalEntityName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityByNameInternal(string companyLegalEntityName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, companyLegalEntityName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of CompanyLegalEntities (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntities")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntities()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntitiesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntities (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntitiesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntitiesByIdentsInternal(companyLegalEntityIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesByIdentsInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO[] companyLegalEntityIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(companyLegalEntityIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO> GetVisualCompanyLegalEntitiesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntity")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntityInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntityByCode")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntityByCodeInternal(companyLegalEntityCode, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityByCodeInternal(string companyLegalEntityCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, companyLegalEntityCode, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntityByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string companyLegalEntityName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntityByNameInternal(companyLegalEntityName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityByNameInternal(string companyLegalEntityName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, companyLegalEntityName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.CompanyLegalEntity>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for CompanyLegalEntity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasCompanyLegalEntityAccess")]
        public virtual bool HasCompanyLegalEntityAccess(HasCompanyLegalEntityAccessAutoRequest hasCompanyLegalEntityAccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = hasCompanyLegalEntityAccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent = hasCompanyLegalEntityAccessAutoRequest.companyLegalEntityIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasCompanyLegalEntityAccessInternal(companyLegalEntityIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasCompanyLegalEntityAccessInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntity;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.CompanyLegalEntity>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove CompanyLegalEntity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveCompanyLegalEntity")]
        public virtual void RemoveCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveCompanyLegalEntityInternal(companyLegalEntityIdent, evaluateData));
        }
        
        protected virtual void RemoveCompanyLegalEntityInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveCompanyLegalEntityInternal(companyLegalEntityIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveCompanyLegalEntityInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData, WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll)
        {
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetById(companyLegalEntityIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save CompanyLegalEntities
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveCompanyLegalEntity")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO SaveCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityStrictDTO companyLegalEntityStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveCompanyLegalEntityInternal(companyLegalEntityStrict, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO SaveCompanyLegalEntityInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityStrictDTO companyLegalEntityStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveCompanyLegalEntityInternal(companyLegalEntityStrict, evaluateData, bll);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO SaveCompanyLegalEntityInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityStrictDTO companyLegalEntityStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData, WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll)
        {
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, companyLegalEntityStrict.Id);
            companyLegalEntityStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        /// <summary>
        /// Get CustomCompanyLegalEntity (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCustomCompanyLegalEntity")]
        public virtual WorkflowSampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO GetCustomCompanyLegalEntity([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO customCompanyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetCustomCompanyLegalEntityInternal(customCompanyLegalEntityIdentity, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CustomCompanyLegalEntityProjectionDTO GetCustomCompanyLegalEntityInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO customCompanyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICustomCompanyLegalEntityBLL bll = evaluateData.Context.Logics.CustomCompanyLegalEntityFactory.Create(WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode.AuthorizationImpersonate);
            WorkflowSampleSystem.Domain.Projections.CustomCompanyLegalEntity domainObject = bll.GetById(customCompanyLegalEntityIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Projections.CustomCompanyLegalEntity>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckCompanyLegalEntityAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasCompanyLegalEntityAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
}
