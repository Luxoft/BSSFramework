namespace Framework.Database.EntityFramework.InlineAudit.DependencyInjection;

public class InlineAuditExtensionSetup : IInlineAuditExtensionSetup
{
    public Func<IInlineAuditExtensionInnerSetup>? CreateInstance { get; private set; }

    public Type? SetupType { get; private set; }

    public IInlineAuditExtensionSetup SetSetupType<TSetup>()
        where TSetup : IInlineAuditExtensionInnerSetup, new()
    {
        this.SetupType = typeof(TSetup);

        this.CreateInstance = () => new TSetup();

        return this;
    }
}
