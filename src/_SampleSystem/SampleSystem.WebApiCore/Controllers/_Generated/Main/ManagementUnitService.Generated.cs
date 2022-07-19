namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class ManagementUnitController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check ManagementUnit access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckManagementUnitAccess")]
        public virtual void CheckManagementUnitAccess(CheckManagementUnitAccessAutoRequest checkManagementUnitAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkManagementUnitAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdent = checkManagementUnitAccessAutoRequest.managementUnitIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckManagementUnitAccessInternal(managementUnitIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckManagementUnitAccessInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnit;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetById(managementUnitIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.ManagementUnit>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get ManagementUnit (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnit")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitInternal(managementUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnit (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitByName")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitByNameInternal(managementUnitName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitByNameInternal(string managementUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnits (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitsByIdentsInternal(managementUnitIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(managementUnitIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnits (FullDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitsByOperation")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitsByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsByOperationInternal(SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitFullDTO> GetFullManagementUnitsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get hierarchical data of type ManagementUnits (FullDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitTreeByOperation")]
        public virtual System.Collections.Generic.List<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.ManagementUnitFullDTO, System.Guid>> GetFullManagementUnitTreeByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitTreeByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.List<Framework.Persistent.HierarchicalNode<SampleSystem.Generated.DTO.ManagementUnitFullDTO, System.Guid>> GetFullManagementUnitTreeByOperationInternal(SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            var tree = bll.GetTree(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Persistent.HierarchicalNodeExtensions.ToList(tree, managementUnit => SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(managementUnit, evaluateData.MappingService));
        }
        
        /// <summary>
        /// Get ManagementUnit (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichManagementUnit")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitInternal(managementUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnit (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichManagementUnitByName")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitByNameInternal(managementUnitName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitByNameInternal(string managementUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitInternal(managementUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnit (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitByName")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitByNameInternal(managementUnitName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitByNameInternal(string managementUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnits (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitsByIdentsInternal(managementUnitIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(managementUnitIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnits (SimpleDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitsByOperation")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitsByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsByOperationInternal(SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitSimpleDTO> GetSimpleManagementUnitsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualManagementUnit")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitInternal(managementUnitIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnit (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualManagementUnitByName")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string managementUnitName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitByNameInternal(managementUnitName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitByNameInternal(string managementUnitName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnit domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, managementUnitName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ManagementUnits (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualManagementUnitsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitsByIdentsInternal(managementUnitIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsByIdentsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO[] managementUnitIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(managementUnitIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnits (VisualDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualManagementUnitsByOperation")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode securityOperationCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitsByOperationInternal(securityOperationCode, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsByOperationInternal(SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.Security.TransferEnumHelper.Convert<SampleSystem.Generated.DTO.SampleSystemManagementUnitSecurityOperationCode, SampleSystem.SampleSystemSecurityOperationCode>(securityOperationCode));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.ManagementUnitVisualDTO> GetVisualManagementUnitsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.ManagementUnit>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for ManagementUnit
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasManagementUnitAccess")]
        public virtual bool HasManagementUnitAccess(HasManagementUnitAccessAutoRequest hasManagementUnitAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasManagementUnitAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdent = hasManagementUnitAccessAutoRequest.managementUnitIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasManagementUnitAccessInternal(managementUnitIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasManagementUnitAccessInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnit;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetById(managementUnitIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.ManagementUnit>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save ManagementUnits
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveManagementUnit")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO SaveManagementUnit([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitStrictDTO managementUnitStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveManagementUnitInternal(managementUnitStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO SaveManagementUnitInternal(SampleSystem.Generated.DTO.ManagementUnitStrictDTO managementUnitStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
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
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckManagementUnitAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasManagementUnitAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
