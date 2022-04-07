using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.WebApiCore.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class WorkflowController : ApiControllerBase<
        IServiceEnvironment<ISampleSystemBLLContext>,
        ISampleSystemBLLContext, EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>
{
    private readonly StartWorkflowJob startWorkflowJob;

    public WorkflowController(IServiceEnvironment<ISampleSystemBLLContext> environment, IExceptionProcessor exceptionProcessor, StartWorkflowJob startWorkflowJob) : base(environment, exceptionProcessor)
    {
        this.startWorkflowJob = startWorkflowJob;
    }

    protected override EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService> GetEvaluatedData(IDBSession session, ISampleSystemBLLContext context) =>
            new(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));


    [HttpPost(nameof(StartJob))]
    public Dictionary<Guid, Guid> StartJob()
    {
        return this.startWorkflowJob.Start();
    }
}
