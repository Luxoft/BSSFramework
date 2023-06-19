using Framework.DomainDriven;

namespace Framework.HangfireCore.JobServices;

public class ScopedJobExecutor : IScopedJobExecutor
{
    private readonly IDBSessionManager dbSessionManager;
    private readonly IHangfireCredentialSettings hangfireSettings;
    private readonly IImpersonateService impersonateService;

    public ScopedJobExecutor(IDBSessionManager dbSessionManager, IHangfireCredentialSettings hangfireSettings, IImpersonateService impersonateService)
    {
        this.dbSessionManager = dbSessionManager;
        this.hangfireSettings = hangfireSettings;
        this.impersonateService = impersonateService;
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> executedTask)
    {
        var cancellationToken = CancellationToken.None;

        try
        {
            return await this.impersonateService.WithImpersonateAsync(this.hangfireSettings.RunAs, executedTask);
        }
        catch
        {
            this.dbSessionManager.TryFaultDbSession();
            throw;
        }
        finally
        {
            await this.dbSessionManager.TryCloseDbSessionAsync(cancellationToken);
        }
    }
}
