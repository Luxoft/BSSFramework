using Framework.Database.InlineAudit.DependencyInjection;

namespace Framework.Database.EntityFramework.InlineAudit.DependencyInjection;

public interface IInlineAuditExtensionInnerSetup
{
    void Initialize(IInlineAuditSetup setup);
}
