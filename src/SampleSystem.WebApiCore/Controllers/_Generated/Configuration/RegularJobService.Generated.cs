namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class RegularJobController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext>, Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        public RegularJobController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check RegularJob access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckRegularJobAccess")]
        public virtual void CheckRegularJobAccess(CheckRegularJobAccessAutoRequest checkRegularJobAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkRegularJobAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdent = checkRegularJobAccessAutoRequest.regularJobIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckRegularJobAccessInternal(regularJobIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckRegularJobAccessInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJob;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.RegularJob domainObject = bll.GetById(regularJobIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.RegularJob>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Configuration.BLL.IConfigurationBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>(session, context, new ConfigurationServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get RegularJob (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullRegularJob")]
        public virtual Framework.Configuration.Generated.DTO.RegularJobFullDTO GetFullRegularJob([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullRegularJobInternal(regularJobIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get RegularJob (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullRegularJobByName")]
        public virtual Framework.Configuration.Generated.DTO.RegularJobFullDTO GetFullRegularJobByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string regularJobName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullRegularJobByNameInternal(regularJobName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobFullDTO GetFullRegularJobByNameInternal(string regularJobName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.RegularJob domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, regularJobName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobFullDTO GetFullRegularJobInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.RegularJob domainObject = bll.GetById(regularJobIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of RegularJobs (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullRegularJobs")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobFullDTO> GetFullRegularJobs()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullRegularJobsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get RegularJobs (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullRegularJobsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobFullDTO> GetFullRegularJobsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.RegularJobIdentityDTO[] regularJobIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullRegularJobsByIdentsInternal(regularJobIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobFullDTO> GetFullRegularJobsByIdentsInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO[] regularJobIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(regularJobIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobFullDTO> GetFullRegularJobsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get RegularJob (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichRegularJob")]
        public virtual Framework.Configuration.Generated.DTO.RegularJobRichDTO GetRichRegularJob([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichRegularJobInternal(regularJobIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get RegularJob (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichRegularJobByName")]
        public virtual Framework.Configuration.Generated.DTO.RegularJobRichDTO GetRichRegularJobByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string regularJobName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichRegularJobByNameInternal(regularJobName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobRichDTO GetRichRegularJobByNameInternal(string regularJobName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.RegularJob domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, regularJobName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobRichDTO GetRichRegularJobInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.RegularJob domainObject = bll.GetById(regularJobIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get RegularJob (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleRegularJob")]
        public virtual Framework.Configuration.Generated.DTO.RegularJobSimpleDTO GetSimpleRegularJob([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleRegularJobInternal(regularJobIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get RegularJob (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleRegularJobByName")]
        public virtual Framework.Configuration.Generated.DTO.RegularJobSimpleDTO GetSimpleRegularJobByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string regularJobName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleRegularJobByNameInternal(regularJobName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobSimpleDTO GetSimpleRegularJobByNameInternal(string regularJobName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.RegularJob domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, regularJobName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobSimpleDTO GetSimpleRegularJobInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.RegularJob domainObject = bll.GetById(regularJobIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of RegularJobs (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleRegularJobs")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobSimpleDTO> GetSimpleRegularJobs()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleRegularJobsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get RegularJobs (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleRegularJobsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobSimpleDTO> GetSimpleRegularJobsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.RegularJobIdentityDTO[] regularJobIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleRegularJobsByIdentsInternal(regularJobIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobSimpleDTO> GetSimpleRegularJobsByIdentsInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO[] regularJobIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(regularJobIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobSimpleDTO> GetSimpleRegularJobsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get RegularJob (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualRegularJob")]
        public virtual Framework.Configuration.Generated.DTO.RegularJobVisualDTO GetVisualRegularJob([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualRegularJobInternal(regularJobIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get RegularJob (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualRegularJobByName")]
        public virtual Framework.Configuration.Generated.DTO.RegularJobVisualDTO GetVisualRegularJobByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string regularJobName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualRegularJobByNameInternal(regularJobName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobVisualDTO GetVisualRegularJobByNameInternal(string regularJobName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.RegularJob domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, regularJobName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobVisualDTO GetVisualRegularJobInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.RegularJob domainObject = bll.GetById(regularJobIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of RegularJobs (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualRegularJobs")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobVisualDTO> GetVisualRegularJobs()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualRegularJobsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get RegularJobs (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualRegularJobsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobVisualDTO> GetVisualRegularJobsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.RegularJobIdentityDTO[] regularJobIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualRegularJobsByIdentsInternal(regularJobIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobVisualDTO> GetVisualRegularJobsByIdentsInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO[] regularJobIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(regularJobIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.RegularJobVisualDTO> GetVisualRegularJobsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.RegularJob>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for RegularJob
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasRegularJobAccess")]
        public virtual bool HasRegularJobAccess(HasRegularJobAccessAutoRequest hasRegularJobAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasRegularJobAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdent = hasRegularJobAccessAutoRequest.regularJobIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasRegularJobAccessInternal(regularJobIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasRegularJobAccessInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJob;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.RegularJob domainObject = bll.GetById(regularJobIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.RegularJob>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove RegularJob
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveRegularJob")]
        public virtual void RemoveRegularJob([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveRegularJobInternal(regularJobIdent, evaluateData));
        }
        
        protected virtual void RemoveRegularJobInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveRegularJobInternal(regularJobIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveRegularJobInternal(Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.IRegularJobBLL bll)
        {
            Framework.Configuration.Domain.RegularJob domainObject = bll.GetById(regularJobIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save RegularJobs
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveRegularJob")]
        public virtual Framework.Configuration.Generated.DTO.RegularJobIdentityDTO SaveRegularJob([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.RegularJobStrictDTO regularJobStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveRegularJobInternal(regularJobStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobIdentityDTO SaveRegularJobInternal(Framework.Configuration.Generated.DTO.RegularJobStrictDTO regularJobStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IRegularJobBLL bll = evaluateData.Context.Logics.RegularJobFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveRegularJobInternal(regularJobStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.RegularJobIdentityDTO SaveRegularJobInternal(Framework.Configuration.Generated.DTO.RegularJobStrictDTO regularJobStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.IRegularJobBLL bll)
        {
            Framework.Configuration.Domain.RegularJob domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, regularJobStrict.Id);
            regularJobStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckRegularJobAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasRegularJobAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.RegularJobIdentityDTO regularJobIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
