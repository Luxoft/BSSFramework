namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class CommandController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public CommandController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check Command access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckCommandAccess")]
        public virtual void CheckCommandAccess(CheckCommandAccessAutoRequest checkCommandAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkCommandAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdent = checkCommandAccessAutoRequest.commandIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckCommandAccessInternal(commandIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckCommandAccessInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.Command;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.Command domainObject = bll.GetById(commandIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.Command>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get Command (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCommand")]
        public virtual Framework.Workflow.Generated.DTO.CommandFullDTO GetFullCommand([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCommandInternal(commandIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Command (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCommandByName")]
        public virtual Framework.Workflow.Generated.DTO.CommandFullDTO GetFullCommandByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string commandName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCommandByNameInternal(commandName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandFullDTO GetFullCommandByNameInternal(string commandName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.Command domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, commandName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandFullDTO GetFullCommandInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.Command domainObject = bll.GetById(commandIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Commands (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCommands")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandFullDTO> GetFullCommands()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCommandsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Commands (FullDTO) by filter (Framework.Workflow.Domain.AvailableCommandFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCommandsByAvailableCommandFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandFullDTO> GetFullCommandsByAvailableCommandFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.AvailableCommandFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCommandsByAvailableCommandFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandFullDTO> GetFullCommandsByAvailableCommandFilterInternal(Framework.Workflow.Generated.DTO.AvailableCommandFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.AvailableCommandFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Commands (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCommandsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandFullDTO> GetFullCommandsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandIdentityDTO[] commandIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCommandsByIdentsInternal(commandIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandFullDTO> GetFullCommandsByIdentsInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO[] commandIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(commandIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Commands (FullDTO) by filter (Framework.Workflow.Domain.CommandRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCommandsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandFullDTO> GetFullCommandsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCommandsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandFullDTO> GetFullCommandsByRootFilterInternal(Framework.Workflow.Generated.DTO.CommandRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.CommandRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandFullDTO> GetFullCommandsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Command (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCommand")]
        public virtual Framework.Workflow.Generated.DTO.CommandRichDTO GetRichCommand([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichCommandInternal(commandIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Command (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCommandByName")]
        public virtual Framework.Workflow.Generated.DTO.CommandRichDTO GetRichCommandByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string commandName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichCommandByNameInternal(commandName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandRichDTO GetRichCommandByNameInternal(string commandName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.Command domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, commandName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandRichDTO GetRichCommandInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.Command domainObject = bll.GetById(commandIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Command (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCommand")]
        public virtual Framework.Workflow.Generated.DTO.CommandSimpleDTO GetSimpleCommand([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCommandInternal(commandIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Command (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCommandByName")]
        public virtual Framework.Workflow.Generated.DTO.CommandSimpleDTO GetSimpleCommandByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string commandName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCommandByNameInternal(commandName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandSimpleDTO GetSimpleCommandByNameInternal(string commandName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.Command domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, commandName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandSimpleDTO GetSimpleCommandInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.Command domainObject = bll.GetById(commandIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Commands (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCommands")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandSimpleDTO> GetSimpleCommands()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCommandsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Commands (SimpleDTO) by filter (Framework.Workflow.Domain.AvailableCommandFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCommandsByAvailableCommandFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandSimpleDTO> GetSimpleCommandsByAvailableCommandFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.AvailableCommandFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCommandsByAvailableCommandFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandSimpleDTO> GetSimpleCommandsByAvailableCommandFilterInternal(Framework.Workflow.Generated.DTO.AvailableCommandFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.AvailableCommandFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Commands (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCommandsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandSimpleDTO> GetSimpleCommandsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandIdentityDTO[] commandIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCommandsByIdentsInternal(commandIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandSimpleDTO> GetSimpleCommandsByIdentsInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO[] commandIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(commandIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Commands (SimpleDTO) by filter (Framework.Workflow.Domain.CommandRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCommandsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandSimpleDTO> GetSimpleCommandsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCommandsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandSimpleDTO> GetSimpleCommandsByRootFilterInternal(Framework.Workflow.Generated.DTO.CommandRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.CommandRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandSimpleDTO> GetSimpleCommandsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Command (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCommand")]
        public virtual Framework.Workflow.Generated.DTO.CommandVisualDTO GetVisualCommand([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCommandInternal(commandIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Command (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCommandByName")]
        public virtual Framework.Workflow.Generated.DTO.CommandVisualDTO GetVisualCommandByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string commandName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCommandByNameInternal(commandName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandVisualDTO GetVisualCommandByNameInternal(string commandName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.Command domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, commandName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandVisualDTO GetVisualCommandInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.Command domainObject = bll.GetById(commandIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Commands (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCommands")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandVisualDTO> GetVisualCommands()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCommandsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Commands (VisualDTO) by filter (Framework.Workflow.Domain.AvailableCommandFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCommandsByAvailableCommandFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandVisualDTO> GetVisualCommandsByAvailableCommandFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.AvailableCommandFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCommandsByAvailableCommandFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandVisualDTO> GetVisualCommandsByAvailableCommandFilterInternal(Framework.Workflow.Generated.DTO.AvailableCommandFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.AvailableCommandFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Commands (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCommandsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandVisualDTO> GetVisualCommandsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandIdentityDTO[] commandIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCommandsByIdentsInternal(commandIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandVisualDTO> GetVisualCommandsByIdentsInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO[] commandIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(commandIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Commands (VisualDTO) by filter (Framework.Workflow.Domain.CommandRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCommandsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandVisualDTO> GetVisualCommandsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCommandsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandVisualDTO> GetVisualCommandsByRootFilterInternal(Framework.Workflow.Generated.DTO.CommandRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.CommandRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.CommandVisualDTO> GetVisualCommandsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.Command>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Command
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasCommandAccess")]
        public virtual bool HasCommandAccess(HasCommandAccessAutoRequest hasCommandAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasCommandAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdent = hasCommandAccessAutoRequest.commandIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasCommandAccessInternal(commandIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasCommandAccessInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.Command;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.Command domainObject = bll.GetById(commandIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.Command>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove Command
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveCommand")]
        public virtual void RemoveCommand([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveCommandInternal(commandIdent, evaluateData));
        }
        
        protected virtual void RemoveCommandInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveCommandInternal(commandIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveCommandInternal(Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.ICommandBLL bll)
        {
            Framework.Workflow.Domain.Definition.Command domainObject = bll.GetById(commandIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save Commands
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveCommand")]
        public virtual Framework.Workflow.Generated.DTO.CommandIdentityDTO SaveCommand([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.CommandStrictDTO commandStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveCommandInternal(commandStrict, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandIdentityDTO SaveCommandInternal(Framework.Workflow.Generated.DTO.CommandStrictDTO commandStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ICommandBLL bll = evaluateData.Context.Logics.CommandFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveCommandInternal(commandStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.CommandIdentityDTO SaveCommandInternal(Framework.Workflow.Generated.DTO.CommandStrictDTO commandStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.ICommandBLL bll)
        {
            Framework.Workflow.Domain.Definition.Command domainObject = bll.GetById(commandStrict.Id, true);
            commandStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckCommandAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasCommandAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.CommandIdentityDTO commandIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
