namespace Framework.Database.EntityFramework.InlineAudit.DependencyInjection;

public interface IInlineAuditExtensionSetup
{
    IInlineAuditExtensionSetup SetSetupType<TSetup>()
        where TSetup : IInlineAuditExtensionInnerSetup, new();
}
