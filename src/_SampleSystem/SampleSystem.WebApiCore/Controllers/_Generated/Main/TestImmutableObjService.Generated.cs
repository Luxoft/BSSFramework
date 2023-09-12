namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class TestImmutableObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check TestImmutableObj access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckTestImmutableObjAccess")]
        public virtual void CheckTestImmutableObjAccess(CheckTestImmutableObjAccessAutoRequest checkTestImmutableObjAccessAutoRequest)
        {
            string securityOperationName = checkTestImmutableObjAccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdent = checkTestImmutableObjAccessAutoRequest.testImmutableObjIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckTestImmutableObjAccessInternal(testImmutableObjIdent, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckTestImmutableObjAccessInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObj;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(securityOperationName, typeof(SampleSystem.SampleSystemSecurityOperation));
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetById(testImmutableObjIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.TestImmutableObj>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get TestImmutableObj (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestImmutableObj")]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjFullDTO GetFullTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestImmutableObjInternal(testImmutableObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjFullDTO GetFullTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetById(testImmutableObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestImmutableObjs (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestImmutableObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjFullDTO> GetFullTestImmutableObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestImmutableObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestImmutableObjs (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestImmutableObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjFullDTO> GetFullTestImmutableObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO[] testImmutableObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestImmutableObjsByIdentsInternal(testImmutableObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjFullDTO> GetFullTestImmutableObjsByIdentsInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO[] testImmutableObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testImmutableObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjFullDTO> GetFullTestImmutableObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestImmutableObj (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestImmutableObj")]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjRichDTO GetRichTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestImmutableObjInternal(testImmutableObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjRichDTO GetRichTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetById(testImmutableObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestImmutableObj (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestImmutableObj")]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO GetSimpleTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestImmutableObjInternal(testImmutableObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO GetSimpleTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetById(testImmutableObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestImmutableObjs (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestImmutableObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO> GetSimpleTestImmutableObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestImmutableObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestImmutableObjs (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestImmutableObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO> GetSimpleTestImmutableObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO[] testImmutableObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestImmutableObjsByIdentsInternal(testImmutableObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO> GetSimpleTestImmutableObjsByIdentsInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO[] testImmutableObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testImmutableObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO> GetSimpleTestImmutableObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for TestImmutableObj
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasTestImmutableObjAccess")]
        public virtual bool HasTestImmutableObjAccess(HasTestImmutableObjAccessAutoRequest hasTestImmutableObjAccessAutoRequest)
        {
            string securityOperationName = hasTestImmutableObjAccessAutoRequest.securityOperationName;
            SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdent = hasTestImmutableObjAccessAutoRequest.testImmutableObjIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasTestImmutableObjAccessInternal(testImmutableObjIdent, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasTestImmutableObjAccessInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObj;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(securityOperationName, typeof(SampleSystem.SampleSystemSecurityOperation));
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetById(testImmutableObjIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.TestImmutableObj>(operation).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save TestImmutableObjs
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveTestImmutableObj")]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjStrictDTO testImmutableObjStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveTestImmutableObjInternal(testImmutableObjStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjStrictDTO testImmutableObjStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveTestImmutableObjInternal(testImmutableObjStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjStrictDTO testImmutableObjStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ITestImmutableObjBLL bll)
        {
            SampleSystem.Domain.TestImmutableObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, testImmutableObjStrict.Id);
            testImmutableObjStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckTestImmutableObjAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasTestImmutableObjAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
