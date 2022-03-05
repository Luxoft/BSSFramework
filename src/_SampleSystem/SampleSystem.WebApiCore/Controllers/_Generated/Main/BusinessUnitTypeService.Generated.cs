namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitTypeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public BusinessUnitTypeController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check BusinessUnitType access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckBusinessUnitTypeAccess")]
        public virtual void CheckBusinessUnitTypeAccess(CheckBusinessUnitTypeAccessAutoRequest checkBusinessUnitTypeAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkBusinessUnitTypeAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdent = checkBusinessUnitTypeAccessAutoRequest.businessUnitTypeIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckBusinessUnitTypeAccessInternal(businessUnitTypeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckBusinessUnitTypeAccessInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitType;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.BusinessUnitType>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get BusinessUnitType (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitType")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypeInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitType (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitTypeByName")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypeByNameInternal(businessUnitTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeByNameInternal(string businessUnitTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitTypeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitTypes (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitTypes")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO> GetFullBusinessUnitTypes()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitTypes (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitTypesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO> GetFullBusinessUnitTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypesByIdentsInternal(businessUnitTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO> GetFullBusinessUnitTypesByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(businessUnitTypeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO> GetFullBusinessUnitTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessUnitType")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitTypeInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitType (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessUnitTypeByName")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitTypeByNameInternal(businessUnitTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeByNameInternal(string businessUnitTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitTypeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitType")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypeInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitType (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitTypeByName")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypeByNameInternal(businessUnitTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeByNameInternal(string businessUnitTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitTypeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitTypes (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitTypes")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO> GetSimpleBusinessUnitTypes()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitTypes (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitTypesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO> GetSimpleBusinessUnitTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypesByIdentsInternal(businessUnitTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO> GetSimpleBusinessUnitTypesByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(businessUnitTypeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO> GetSimpleBusinessUnitTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitType")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypeInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitType (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitTypeByName")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessUnitTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypeByNameInternal(businessUnitTypeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeByNameInternal(string businessUnitTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessUnitTypeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessUnitTypes (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitTypes")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO> GetVisualBusinessUnitTypes()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessUnitTypes (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitTypesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO> GetVisualBusinessUnitTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypesByIdentsInternal(businessUnitTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO> GetVisualBusinessUnitTypesByIdentsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO[] businessUnitTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(businessUnitTypeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO> GetVisualBusinessUnitTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.BusinessUnitType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for BusinessUnitType
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasBusinessUnitTypeAccess")]
        public virtual bool HasBusinessUnitTypeAccess(HasBusinessUnitTypeAccessAutoRequest hasBusinessUnitTypeAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasBusinessUnitTypeAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdent = hasBusinessUnitTypeAccessAutoRequest.businessUnitTypeIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasBusinessUnitTypeAccessInternal(businessUnitTypeIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasBusinessUnitTypeAccessInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitType;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetById(businessUnitTypeIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.BusinessUnitType>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save BusinessUnitTypes
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveBusinessUnitType")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO SaveBusinessUnitType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeStrictDTO businessUnitTypeStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveBusinessUnitTypeInternal(businessUnitTypeStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO SaveBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeStrictDTO businessUnitTypeStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveBusinessUnitTypeInternal(businessUnitTypeStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO SaveBusinessUnitTypeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeStrictDTO businessUnitTypeStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IBusinessUnitTypeBLL bll)
        {
            SampleSystem.Domain.BusinessUnitType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, businessUnitTypeStrict.Id);
            businessUnitTypeStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckBusinessUnitTypeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasBusinessUnitTypeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
