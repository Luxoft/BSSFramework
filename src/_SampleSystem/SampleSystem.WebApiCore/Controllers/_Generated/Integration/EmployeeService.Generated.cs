namespace SampleSystem.WebApiCore.Controllers.Integration
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/v{version:apiVersion}/[controller]")]
    public partial class EmployeeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public EmployeeController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Save Employee
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveEmployee")]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIntegrationRichDTO employee)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveEmployeeInternal(employee, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIntegrationRichDTO employee, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.Employee;
            Framework.DomainDriven.BLL.Security.AuthorizationBLLContextExtensions.CheckAccess(evaluateData.Context.Authorization, SampleSystem.BLL.SampleSystemSecurityOperation.SystemIntegration);
            return this.SaveEmployeeInternal(employee, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIntegrationRichDTO employee, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IEmployeeBLL bll)
        {
            SampleSystem.Domain.Employee domainObject = bll.GetById(employee.Id, false);
            if (object.ReferenceEquals(domainObject, null))
            {
                domainObject = new SampleSystem.Domain.Employee();
            }
            employee.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Insert(domainObject, employee.Id);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
