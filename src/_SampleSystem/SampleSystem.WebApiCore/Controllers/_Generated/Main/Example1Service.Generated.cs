namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class Example1Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Example1 (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.Example1FullDTO GetFullExample1([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExample1Internal(example1Identity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Example1FullDTO GetFullExample1Internal(SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Example1 domainObject = bll.GetById(example1Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Example1>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Example1s (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Example1FullDTO> GetFullExample1s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExample1sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Example1s (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Example1FullDTO> GetFullExample1sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Example1IdentityDTO[] example1Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExample1sByIdentsInternal(example1Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Example1FullDTO> GetFullExample1sByIdentsInternal(SampleSystem.Generated.DTO.Example1IdentityDTO[] example1Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(example1Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Example1>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Example1FullDTO> GetFullExample1sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Example1>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Example1 (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.Example1RichDTO GetRichExample1([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichExample1Internal(example1Identity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Example1RichDTO GetRichExample1Internal(SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Example1 domainObject = bll.GetById(example1Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Example1>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Example1 (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.Example1SimpleDTO GetSimpleExample1([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExample1Internal(example1Identity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Example1SimpleDTO GetSimpleExample1Internal(SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Example1 domainObject = bll.GetById(example1Identity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Example1>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Example1s (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Example1SimpleDTO> GetSimpleExample1s()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExample1sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Example1s (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Example1SimpleDTO> GetSimpleExample1sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Example1IdentityDTO[] example1Idents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExample1sByIdentsInternal(example1Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Example1SimpleDTO> GetSimpleExample1sByIdentsInternal(SampleSystem.Generated.DTO.Example1IdentityDTO[] example1Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(example1Idents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Example1>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.Example1SimpleDTO> GetSimpleExample1sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.Example1>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save Example1s
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.Example1IdentityDTO SaveExample1([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Example1StrictDTO example1Strict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveExample1Internal(example1Strict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Example1IdentityDTO SaveExample1Internal(SampleSystem.Generated.DTO.Example1StrictDTO example1Strict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.Edit);
            return this.SaveExample1Internal(example1Strict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.Example1IdentityDTO SaveExample1Internal(SampleSystem.Generated.DTO.Example1StrictDTO example1Strict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IExample1BLL bll)
        {
            SampleSystem.Domain.Example1 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, example1Strict.Id);
            example1Strict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
        
        /// <summary>
        /// Update Example1
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.Example1IdentityDTO UpdateExample1([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Example1UpdateDTO example1Update)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.UpdateExample1Internal(example1Update, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Example1IdentityDTO UpdateExample1Internal(SampleSystem.Generated.DTO.Example1UpdateDTO example1Update, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.Edit);
            SampleSystem.Domain.Example1 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, example1Update.Id);
            example1Update.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
