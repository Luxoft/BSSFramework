namespace Framework.Database.EntityFramework.EnversAudit.DependencyInjection;

public interface IEnversAuditExtensionSetup
{
    IEnversAuditExtensionSetup SetSetupType<TSetup>()
        where TSetup : IEnversAuditExtensionInnerSetup, new();
}
