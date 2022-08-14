namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class TestPerformanceObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check TestPerformanceObject access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckTestPerformanceObjectAccess")]
        public virtual void CheckTestPerformanceObjectAccess(CheckTestPerformanceObjectAccessAutoRequest checkTestPerformanceObjectAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkTestPerformanceObjectAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdent = checkTestPerformanceObjectAccessAutoRequest.testPerformanceObjectIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckTestPerformanceObjectAccessInternal(testPerformanceObjectIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckTestPerformanceObjectAccessInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObject;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.TestPerformanceObject>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPerformanceObject")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPerformanceObjectByName")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPerformanceObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectByNameInternal(testPerformanceObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectByNameInternal(string testPerformanceObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPerformanceObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestPerformanceObjects (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPerformanceObjects")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO> GetFullTestPerformanceObjects()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObjects (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPerformanceObjectsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO> GetFullTestPerformanceObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectsByIdentsInternal(testPerformanceObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO> GetFullTestPerformanceObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testPerformanceObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO> GetFullTestPerformanceObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestPerformanceObject")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestPerformanceObjectInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestPerformanceObjectByName")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPerformanceObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestPerformanceObjectByNameInternal(testPerformanceObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectByNameInternal(string testPerformanceObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPerformanceObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPerformanceObject")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPerformanceObjectByName")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPerformanceObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectByNameInternal(testPerformanceObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectByNameInternal(string testPerformanceObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPerformanceObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestPerformanceObjects (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPerformanceObjects")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO> GetSimpleTestPerformanceObjects()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObjects (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPerformanceObjectsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO> GetSimpleTestPerformanceObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectsByIdentsInternal(testPerformanceObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO> GetSimpleTestPerformanceObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testPerformanceObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO> GetSimpleTestPerformanceObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPerformanceObject")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPerformanceObjectByName")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testPerformanceObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectByNameInternal(testPerformanceObjectName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectByNameInternal(string testPerformanceObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testPerformanceObjectName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestPerformanceObjects (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPerformanceObjects")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO> GetVisualTestPerformanceObjects()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestPerformanceObjects (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPerformanceObjectsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO> GetVisualTestPerformanceObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectsByIdentsInternal(testPerformanceObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO> GetVisualTestPerformanceObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO[] testPerformanceObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testPerformanceObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO> GetVisualTestPerformanceObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestPerformanceObject>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for TestPerformanceObject
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasTestPerformanceObjectAccess")]
        public virtual bool HasTestPerformanceObjectAccess(HasTestPerformanceObjectAccessAutoRequest hasTestPerformanceObjectAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasTestPerformanceObjectAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdent = hasTestPerformanceObjectAccessAutoRequest.testPerformanceObjectIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasTestPerformanceObjectAccessInternal(testPerformanceObjectIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasTestPerformanceObjectAccessInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObject;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetById(testPerformanceObjectIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.TestPerformanceObject>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckTestPerformanceObjectAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasTestPerformanceObjectAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
