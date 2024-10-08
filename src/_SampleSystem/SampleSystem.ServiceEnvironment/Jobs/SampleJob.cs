﻿using Framework.DomainDriven.Jobs;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Hangfire;

using Microsoft.Extensions.Logging;

using SampleSystem.Domain;

namespace SampleSystem.ServiceEnvironment.Jobs;

[DisableConcurrentExecution(timeoutInSeconds: 1000)]
public class SampleJob([DisabledSecurity] IRepository<TestJobObject> testRepository, ILogger<SampleJob> logger, ICurrentUser currentUser) : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var currentUserName = currentUser.Name;

        logger.LogInformation("Job executed");

        await testRepository.SaveAsync(new TestJobObject(), cancellationToken);
    }
}
