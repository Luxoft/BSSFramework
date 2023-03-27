namespace Automation.Utils;

public interface IPermissionDefinition
{
    IEnumerable<Tuple<string, Guid>> GetEntities();

    string GetRoleName();
}
