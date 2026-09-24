namespace Framework.Database.EntityFramework.EnversAudit.DependencyInjection;

public interface IEnversAuditExtensionInnerSetup
{
    void Initialize(IEnversAuditSetup setup);
}
