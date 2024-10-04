namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class TestJobObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestJobObject (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestJobObjectFullDTO GetFullTestJobObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestJobObjectInternal(testJobObjectIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestJobObjectFullDTO GetFullTestJobObjectInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestJobObject domainObject = bll.GetById(testJobObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestJobObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestJobObjects (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestJobObjectFullDTO> GetFullTestJobObjects()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestJobObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestJobObjects (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestJobObjectFullDTO> GetFullTestJobObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestJobObjectIdentityDTO[] testJobObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestJobObjectsByIdentsInternal(testJobObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestJobObjectFullDTO> GetFullTestJobObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO[] testJobObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testJobObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestJobObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestJobObjectFullDTO> GetFullTestJobObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestJobObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestJobObject (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestJobObjectRichDTO GetRichTestJobObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestJobObjectInternal(testJobObjectIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestJobObjectRichDTO GetRichTestJobObjectInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestJobObject domainObject = bll.GetById(testJobObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestJobObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestJobObject (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestJobObjectSimpleDTO GetSimpleTestJobObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestJobObjectInternal(testJobObjectIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestJobObjectSimpleDTO GetSimpleTestJobObjectInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestJobObject domainObject = bll.GetById(testJobObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestJobObject>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestJobObjects (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestJobObjectSimpleDTO> GetSimpleTestJobObjects()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestJobObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestJobObjects (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestJobObjectSimpleDTO> GetSimpleTestJobObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestJobObjectIdentityDTO[] testJobObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestJobObjectsByIdentsInternal(testJobObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestJobObjectSimpleDTO> GetSimpleTestJobObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO[] testJobObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testJobObjectIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestJobObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestJobObjectSimpleDTO> GetSimpleTestJobObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.TestJobObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save TestJobObjects
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestJobObjectIdentityDTO SaveTestJobObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestJobObjectStrictDTO testJobObjectStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveTestJobObjectInternal(testJobObjectStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestJobObjectIdentityDTO SaveTestJobObjectInternal(SampleSystem.Generated.DTO.TestJobObjectStrictDTO testJobObjectStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
            return this.SaveTestJobObjectInternal(testJobObjectStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.TestJobObjectIdentityDTO SaveTestJobObjectInternal(SampleSystem.Generated.DTO.TestJobObjectStrictDTO testJobObjectStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ITestJobObjectBLL bll)
        {
            SampleSystem.Domain.TestJobObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, testJobObjectStrict.Id);
            testJobObjectStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
