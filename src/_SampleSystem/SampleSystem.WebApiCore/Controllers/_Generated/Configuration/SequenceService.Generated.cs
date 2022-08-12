namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class SequenceController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        /// <summary>
        /// Check Sequence access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckSequenceAccess")]
        public virtual void CheckSequenceAccess(CheckSequenceAccessAutoRequest checkSequenceAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkSequenceAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent = checkSequenceAccessAutoRequest.sequenceIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckSequenceAccessInternal(sequenceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckSequenceAccessInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.Sequence;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.Sequence domainObject = bll.GetById(sequenceIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Sequence>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Create Sequence by model (SequenceCreateModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CreateSequence")]
        public virtual Framework.Configuration.Generated.DTO.SequenceRichDTO CreateSequence([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceCreateModelStrictDTO sequenceCreateModel)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CreateSequenceInternal(sequenceCreateModel, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceRichDTO CreateSequenceInternal(Framework.Configuration.Generated.DTO.SequenceCreateModelStrictDTO sequenceCreateModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            Framework.Configuration.Domain.SequenceCreateModel createModel = sequenceCreateModel.ToDomainObject(evaluateData.MappingService);
            Framework.Configuration.Domain.Sequence domainObject = bll.Create(createModel);
            bll.CheckAccess(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Sequence (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSequence")]
        public virtual Framework.Configuration.Generated.DTO.SequenceFullDTO GetFullSequence([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSequenceInternal(sequenceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Sequence (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSequenceByName")]
        public virtual Framework.Configuration.Generated.DTO.SequenceFullDTO GetFullSequenceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string sequenceName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSequenceByNameInternal(sequenceName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceFullDTO GetFullSequenceByNameInternal(string sequenceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Sequence domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, sequenceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceFullDTO GetFullSequenceInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Sequence domainObject = bll.GetById(sequenceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Sequences (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSequences")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceFullDTO> GetFullSequences()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSequencesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Sequences (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSequencesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceFullDTO> GetFullSequencesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceIdentityDTO[] sequenceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSequencesByIdentsInternal(sequenceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceFullDTO> GetFullSequencesByIdentsInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO[] sequenceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(sequenceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Sequences (FullDTO) by filter (Framework.Configuration.Domain.SequenceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSequencesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceFullDTO> GetFullSequencesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSequencesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceFullDTO> GetFullSequencesByRootFilterInternal(Framework.Configuration.Generated.DTO.SequenceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SequenceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceFullDTO> GetFullSequencesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Sequence (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSequence")]
        public virtual Framework.Configuration.Generated.DTO.SequenceRichDTO GetRichSequence([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichSequenceInternal(sequenceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Sequence (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSequenceByName")]
        public virtual Framework.Configuration.Generated.DTO.SequenceRichDTO GetRichSequenceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string sequenceName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichSequenceByNameInternal(sequenceName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceRichDTO GetRichSequenceByNameInternal(string sequenceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Sequence domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, sequenceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceRichDTO GetRichSequenceInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Sequence domainObject = bll.GetById(sequenceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Sequence (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSequence")]
        public virtual Framework.Configuration.Generated.DTO.SequenceSimpleDTO GetSimpleSequence([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSequenceInternal(sequenceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Sequence (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSequenceByName")]
        public virtual Framework.Configuration.Generated.DTO.SequenceSimpleDTO GetSimpleSequenceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string sequenceName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSequenceByNameInternal(sequenceName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceSimpleDTO GetSimpleSequenceByNameInternal(string sequenceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Sequence domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, sequenceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceSimpleDTO GetSimpleSequenceInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Sequence domainObject = bll.GetById(sequenceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Sequences (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSequences")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceSimpleDTO> GetSimpleSequences()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSequencesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Sequences (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSequencesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceSimpleDTO> GetSimpleSequencesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceIdentityDTO[] sequenceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSequencesByIdentsInternal(sequenceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceSimpleDTO> GetSimpleSequencesByIdentsInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO[] sequenceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(sequenceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Sequences (SimpleDTO) by filter (Framework.Configuration.Domain.SequenceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSequencesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceSimpleDTO> GetSimpleSequencesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSequencesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceSimpleDTO> GetSimpleSequencesByRootFilterInternal(Framework.Configuration.Generated.DTO.SequenceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SequenceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceSimpleDTO> GetSimpleSequencesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Sequence (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSequence")]
        public virtual Framework.Configuration.Generated.DTO.SequenceVisualDTO GetVisualSequence([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSequenceInternal(sequenceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Sequence (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSequenceByName")]
        public virtual Framework.Configuration.Generated.DTO.SequenceVisualDTO GetVisualSequenceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string sequenceName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSequenceByNameInternal(sequenceName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceVisualDTO GetVisualSequenceByNameInternal(string sequenceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Sequence domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, sequenceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceVisualDTO GetVisualSequenceInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Sequence domainObject = bll.GetById(sequenceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Sequences (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSequences")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceVisualDTO> GetVisualSequences()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSequencesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Sequences (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSequencesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceVisualDTO> GetVisualSequencesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceIdentityDTO[] sequenceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSequencesByIdentsInternal(sequenceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceVisualDTO> GetVisualSequencesByIdentsInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO[] sequenceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(sequenceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Sequences (VisualDTO) by filter (Framework.Configuration.Domain.SequenceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSequencesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceVisualDTO> GetVisualSequencesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSequencesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceVisualDTO> GetVisualSequencesByRootFilterInternal(Framework.Configuration.Generated.DTO.SequenceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SequenceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SequenceVisualDTO> GetVisualSequencesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Sequence>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Sequence
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasSequenceAccess")]
        public virtual bool HasSequenceAccess(HasSequenceAccessAutoRequest hasSequenceAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasSequenceAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent = hasSequenceAccessAutoRequest.sequenceIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasSequenceAccessInternal(sequenceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasSequenceAccessInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.Sequence;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.Sequence domainObject = bll.GetById(sequenceIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Sequence>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove Sequence
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveSequence")]
        public virtual void RemoveSequence([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemoveSequenceInternal(sequenceIdent, evaluateData));
        }
        
        protected virtual void RemoveSequenceInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveSequenceInternal(sequenceIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveSequenceInternal(Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.ISequenceBLL bll)
        {
            Framework.Configuration.Domain.Sequence domainObject = bll.GetById(sequenceIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save Sequences
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveSequence")]
        public virtual Framework.Configuration.Generated.DTO.SequenceIdentityDTO SaveSequence([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SequenceStrictDTO sequenceStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveSequenceInternal(sequenceStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceIdentityDTO SaveSequenceInternal(Framework.Configuration.Generated.DTO.SequenceStrictDTO sequenceStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISequenceBLL bll = evaluateData.Context.Logics.SequenceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveSequenceInternal(sequenceStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SequenceIdentityDTO SaveSequenceInternal(Framework.Configuration.Generated.DTO.SequenceStrictDTO sequenceStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.ISequenceBLL bll)
        {
            Framework.Configuration.Domain.Sequence domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, sequenceStrict.Id);
            sequenceStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckSequenceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasSequenceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.SequenceIdentityDTO sequenceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
