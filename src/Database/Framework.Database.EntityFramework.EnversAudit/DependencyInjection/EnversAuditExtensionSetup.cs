namespace Framework.Database.EntityFramework.EnversAudit.DependencyInjection;

public class EnversAuditExtensionSetup : IEnversAuditExtensionSetup
{
    public Func<IEnversAuditExtensionInnerSetup>? CreateInstance { get; private set; }

    public Type? SetupType { get; private set; }

    public IEnversAuditExtensionSetup SetSetupType<TSetup>()
        where TSetup : IEnversAuditExtensionInnerSetup, new()
    {
        this.SetupType = typeof(TSetup);

        this.CreateInstance = () => new TSetup();

        return this;
    }
}
