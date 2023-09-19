namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class TestRootSecurityObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check TestRootSecurityObj access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckTestRootSecurityObjAccess")]
        public virtual void CheckTestRootSecurityObjAccess(CheckTestRootSecurityObjAccessAutoRequest checkTestRootSecurityObjAccessAutoRequest)
        {
            string securityOperationName = checkTestRootSecurityObjAccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdent = checkTestRootSecurityObjAccessAutoRequest.testRootSecurityObjIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckTestRootSecurityObjAccessInternal(testRootSecurityObjIdent, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckTestRootSecurityObjAccessInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObj;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(SampleSystem.SampleSystemSecurityOperation), securityOperationName);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderBaseExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.TestRootSecurityObj>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestRootSecurityObj")]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestRootSecurityObjInternal(testRootSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestRootSecurityObjByName")]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testRootSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestRootSecurityObjByNameInternal(testRootSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObjByNameInternal(string testRootSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testRootSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObjInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestRootSecurityObjs (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestRootSecurityObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO> GetFullTestRootSecurityObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestRootSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObjs (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestRootSecurityObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO> GetFullTestRootSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestRootSecurityObjsByIdentsInternal(testRootSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO> GetFullTestRootSecurityObjsByIdentsInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testRootSecurityObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO> GetFullTestRootSecurityObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestRootSecurityObj")]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestRootSecurityObjInternal(testRootSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestRootSecurityObjByName")]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testRootSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestRootSecurityObjByNameInternal(testRootSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObjByNameInternal(string testRootSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testRootSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObjInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestRootSecurityObj")]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestRootSecurityObjInternal(testRootSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestRootSecurityObjByName")]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testRootSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestRootSecurityObjByNameInternal(testRootSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObjByNameInternal(string testRootSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testRootSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObjInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestRootSecurityObjs (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestRootSecurityObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO> GetSimpleTestRootSecurityObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestRootSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObjs (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestRootSecurityObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO> GetSimpleTestRootSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestRootSecurityObjsByIdentsInternal(testRootSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO> GetSimpleTestRootSecurityObjsByIdentsInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testRootSecurityObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO> GetSimpleTestRootSecurityObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestRootSecurityObj")]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestRootSecurityObjInternal(testRootSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestRootSecurityObjByName")]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testRootSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestRootSecurityObjByNameInternal(testRootSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObjByNameInternal(string testRootSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testRootSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObjInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestRootSecurityObjs (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestRootSecurityObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO> GetVisualTestRootSecurityObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestRootSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObjs (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestRootSecurityObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO> GetVisualTestRootSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestRootSecurityObjsByIdentsInternal(testRootSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO> GetVisualTestRootSecurityObjsByIdentsInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testRootSecurityObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO> GetVisualTestRootSecurityObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestRootSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for TestRootSecurityObj
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasTestRootSecurityObjAccess")]
        public virtual bool HasTestRootSecurityObjAccess(HasTestRootSecurityObjAccessAutoRequest hasTestRootSecurityObjAccessAutoRequest)
        {
            string securityOperationName = hasTestRootSecurityObjAccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdent = hasTestRootSecurityObjAccessAutoRequest.testRootSecurityObjIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasTestRootSecurityObjAccessInternal(testRootSecurityObjIdent, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasTestRootSecurityObjAccessInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObj;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(SampleSystem.SampleSystemSecurityOperation), securityOperationName);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.TestRootSecurityObj>(operation).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckTestRootSecurityObjAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasTestRootSecurityObjAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
