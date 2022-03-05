using System;

using Framework.CustomReports.WebApi;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

using SampleSystem.WebApiCore.CustomReports;

namespace SampleSystem.WebApiCore.Controllers.Report
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("reportApi/v{version:apiVersion}/[controller]")]
    public class SampleSystemGenericReportController : GenericReportControllerBase<SampleSystemCustomReportsServiceEnvironment, ISampleSystemBLLContext,
        PersistentDomainObjectBase,
        SampleSystemSecurityOperationCode, Guid, ISampleSystemDTOMappingService>
    {
        public SampleSystemGenericReportController(SampleSystemCustomReportsServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
            : base(environment, exceptionProcessor)
        {
        }

        protected override EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService> GetEvaluatedData(IDBSession session, ISampleSystemBLLContext context)
        {
            return new EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>(
                session,
                context,
                new SampleSystemServerPrimitiveDTOMappingService(context));
        }
    }
}
