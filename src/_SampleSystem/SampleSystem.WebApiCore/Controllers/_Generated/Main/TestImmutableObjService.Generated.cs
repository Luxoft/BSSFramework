namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class TestImmutableObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestImmutableObj (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjFullDTO GetFullTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestImmutableObjInternal(testImmutableObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjFullDTO GetFullTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetById(testImmutableObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestImmutableObjs (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjFullDTO> GetFullTestImmutableObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestImmutableObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestImmutableObjs (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjFullDTO> GetFullTestImmutableObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO[] testImmutableObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestImmutableObjsByIdentsInternal(testImmutableObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjFullDTO> GetFullTestImmutableObjsByIdentsInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO[] testImmutableObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testImmutableObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjFullDTO> GetFullTestImmutableObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestImmutableObj (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjRichDTO GetRichTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestImmutableObjInternal(testImmutableObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjRichDTO GetRichTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetById(testImmutableObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestImmutableObj (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO GetSimpleTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestImmutableObjInternal(testImmutableObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO GetSimpleTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetById(testImmutableObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestImmutableObjs (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO> GetSimpleTestImmutableObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestImmutableObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestImmutableObjs (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO> GetSimpleTestImmutableObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO[] testImmutableObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestImmutableObjsByIdentsInternal(testImmutableObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO> GetSimpleTestImmutableObjsByIdentsInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO[] testImmutableObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testImmutableObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO> GetSimpleTestImmutableObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestImmutableObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save TestImmutableObjs
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjStrictDTO testImmutableObjStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveTestImmutableObjInternal(testImmutableObjStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO SaveTestImmutableObjInternal(SampleSystem.Generated.DTO.TestImmutableObjStrictDTO testImmutableObjStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
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
}
