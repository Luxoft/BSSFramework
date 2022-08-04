namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class TestPlainAuthObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check TestPlainAuthObject access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckTestPlainAuthObjectAccess")]
        public virtual void CheckTestPlainAuthObjectAccess(CheckTestPlainAuthObjectAccessAutoRequest checkTestPlainAuthObjectAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkTestPlainAuthObjectAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdent = checkTestPlainAuthObjectAccessAutoRequest.testPlainAuthObjectIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckTestPlainAuthObjectAccessInternal(testPlainAuthObjectIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckTestPlainAuthObjectAccessInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObject;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetById(testPlainAuthObjectIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.TestPlainAuthObject>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPlainAuthObject")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO GetFullTestPlainAuthObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestPlainAuthObjectInternal(testPlainAuthObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPlainAuthObjectByName")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO GetFullTestPlainAuthObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPlainAuthObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestPlainAuthObjectByNameInternal(testPlainAuthObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO GetFullTestPlainAuthObjectByNameInternal(string testPlainAuthObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPlainAuthObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO GetFullTestPlainAuthObjectInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetById(testPlainAuthObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestPlainAuthObjects (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPlainAuthObjects")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO> GetFullTestPlainAuthObjects()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestPlainAuthObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestPlainAuthObjects (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPlainAuthObjectsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO> GetFullTestPlainAuthObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO[] testPlainAuthObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestPlainAuthObjectsByIdentsInternal(testPlainAuthObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO> GetFullTestPlainAuthObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO[] testPlainAuthObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testPlainAuthObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO> GetFullTestPlainAuthObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestPlainAuthObject")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectRichDTO GetRichTestPlainAuthObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestPlainAuthObjectInternal(testPlainAuthObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestPlainAuthObjectByName")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectRichDTO GetRichTestPlainAuthObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPlainAuthObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestPlainAuthObjectByNameInternal(testPlainAuthObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectRichDTO GetRichTestPlainAuthObjectByNameInternal(string testPlainAuthObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPlainAuthObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectRichDTO GetRichTestPlainAuthObjectInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetById(testPlainAuthObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPlainAuthObject")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO GetSimpleTestPlainAuthObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestPlainAuthObjectInternal(testPlainAuthObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPlainAuthObjectByName")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO GetSimpleTestPlainAuthObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPlainAuthObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestPlainAuthObjectByNameInternal(testPlainAuthObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO GetSimpleTestPlainAuthObjectByNameInternal(string testPlainAuthObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPlainAuthObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO GetSimpleTestPlainAuthObjectInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetById(testPlainAuthObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestPlainAuthObjects (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPlainAuthObjects")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO> GetSimpleTestPlainAuthObjects()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestPlainAuthObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestPlainAuthObjects (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPlainAuthObjectsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO> GetSimpleTestPlainAuthObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO[] testPlainAuthObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestPlainAuthObjectsByIdentsInternal(testPlainAuthObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO> GetSimpleTestPlainAuthObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO[] testPlainAuthObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testPlainAuthObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO> GetSimpleTestPlainAuthObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPlainAuthObject")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO GetVisualTestPlainAuthObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestPlainAuthObjectInternal(testPlainAuthObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPlainAuthObjectByName")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO GetVisualTestPlainAuthObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPlainAuthObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestPlainAuthObjectByNameInternal(testPlainAuthObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO GetVisualTestPlainAuthObjectByNameInternal(string testPlainAuthObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPlainAuthObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO GetVisualTestPlainAuthObjectInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetById(testPlainAuthObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestPlainAuthObjects (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPlainAuthObjects")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO> GetVisualTestPlainAuthObjects()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestPlainAuthObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestPlainAuthObjects (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPlainAuthObjectsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO> GetVisualTestPlainAuthObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO[] testPlainAuthObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestPlainAuthObjectsByIdentsInternal(testPlainAuthObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO> GetVisualTestPlainAuthObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO[] testPlainAuthObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testPlainAuthObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO> GetVisualTestPlainAuthObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPlainAuthObject>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for TestPlainAuthObject
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasTestPlainAuthObjectAccess")]
        public virtual bool HasTestPlainAuthObjectAccess(HasTestPlainAuthObjectAccessAutoRequest hasTestPlainAuthObjectAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasTestPlainAuthObjectAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdent = hasTestPlainAuthObjectAccessAutoRequest.testPlainAuthObjectIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasTestPlainAuthObjectAccessInternal(testPlainAuthObjectIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasTestPlainAuthObjectAccessInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObject;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetById(testPlainAuthObjectIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.TestPlainAuthObject>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckTestPlainAuthObjectAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasTestPlainAuthObjectAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
