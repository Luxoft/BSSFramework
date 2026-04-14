namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class TestRootSecurityObjController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestRootSecurityObj (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestRootSecurityObjInternal(testRootSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testRootSecurityObjName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestRootSecurityObjByNameInternal(testRootSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObjByNameInternal(string testRootSecurityObjName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestDependency.TestRootSecurityObj domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testRootSecurityObjName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObjInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestDependency.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestRootSecurityObjs (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO> GetFullTestRootSecurityObjs()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestRootSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObjs (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO> GetFullTestRootSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestRootSecurityObjsByIdentsInternal(testRootSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO> GetFullTestRootSecurityObjsByIdentsInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testRootSecurityObjIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO> GetFullTestRootSecurityObjsInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichTestRootSecurityObjInternal(testRootSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testRootSecurityObjName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichTestRootSecurityObjByNameInternal(testRootSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObjByNameInternal(string testRootSecurityObjName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestDependency.TestRootSecurityObj domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testRootSecurityObjName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObjInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestDependency.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestRootSecurityObjInternal(testRootSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testRootSecurityObjName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestRootSecurityObjByNameInternal(testRootSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObjByNameInternal(string testRootSecurityObjName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestDependency.TestRootSecurityObj domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testRootSecurityObjName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObjInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestDependency.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestRootSecurityObjs (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO> GetSimpleTestRootSecurityObjs()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestRootSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObjs (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO> GetSimpleTestRootSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestRootSecurityObjsByIdentsInternal(testRootSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO> GetSimpleTestRootSecurityObjsByIdentsInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testRootSecurityObjIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO> GetSimpleTestRootSecurityObjsInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualTestRootSecurityObjInternal(testRootSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testRootSecurityObjName)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualTestRootSecurityObjByNameInternal(testRootSecurityObjName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObjByNameInternal(string testRootSecurityObjName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestDependency.TestRootSecurityObj domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testRootSecurityObjName, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObjInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestDependency.TestRootSecurityObj domainObject = bll.GetById(testRootSecurityObjIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestRootSecurityObjs (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO> GetVisualTestRootSecurityObjs()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualTestRootSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestRootSecurityObjs (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO> GetVisualTestRootSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualTestRootSecurityObjsByIdentsInternal(testRootSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO> GetVisualTestRootSecurityObjsByIdentsInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO[] testRootSecurityObjIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testRootSecurityObjIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO> GetVisualTestRootSecurityObjsInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.TestDependency.TestRootSecurityObj>(Framework.BLL.Domain.DTO.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
