using Framework.AutomationCore.Services;

namespace Framework.AutomationCore.ServerManagement;

public interface ISqlServerDatabase
{
    string Name { get; }

    void Validate(DatabaseFileInfo fileInfo);
}
