using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.Logging;

using SampleSystem.BLL.Core.Jobs;
using SampleSystem.Domain;

namespace SampleSystem.BLL.Jobs;

public class SampleJob([DisabledSecurity] IRepository<TestJobObject> testRepository, ILogger<SampleJob> logger, ICurrentUser currentUser) : ISampleJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var currentUserName = currentUser.Name;

        logger.LogInformation("Job executed");

        await testRepository.SaveAsync(new TestJobObject(), cancellationToken);
    }
}
