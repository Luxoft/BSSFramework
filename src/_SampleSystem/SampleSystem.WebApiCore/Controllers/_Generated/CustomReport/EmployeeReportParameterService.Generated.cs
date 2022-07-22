namespace SampleSystem.WebApiCore.Controllers.CustomReport
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("reportAuditApi/v{version:apiVersion}/[controller]")]
    public partial class EmployeeReportParameterController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get EmployeeReport custom report
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeReport")]
        public virtual Microsoft.AspNetCore.Mvc.FileStreamResult GetEmployeeReport([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeReportParameterStrictDTO modelDTO)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => {
            SampleSystem.CustomReports.Employee.EmployeeReportParameter customReportParameter = new SampleSystem.CustomReports.Employee.EmployeeReportParameter();

            modelDTO.MapToDomainObject(evaluateData.MappingService, customReportParameter);

            SampleSystem.CustomReports.BLL.EmployeeReportBLL customReport = new SampleSystem.CustomReports.BLL.EmployeeReportBLL(evaluateData.Context);

            Framework.CustomReports.Domain.IReportStream reportStreamResult = customReport.GetReportStream(customReportParameter);

            return this.GetReportResult(reportStreamResult);
});
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
    }
}
