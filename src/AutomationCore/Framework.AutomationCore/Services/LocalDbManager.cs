using MartinCostello.SqlLocalDb;

namespace Framework.AutomationCore.Services;

public class LocalDbManager
{
    public void CreateLocalDb(string instanceName)
    {
        using (var localDb = new SqlLocalDbApi())
        {
            localDb.GetInstances()

            var instanceInfo = localDb.GetInstanceInfo(instanceName);
            localDb.AutomaticallyDeleteInstanceFiles = true;

            if (!instanceInfo.Exists)
            {
                localDb.CreateInstance(instanceInfo.Name);
            }

            if (!instanceInfo.IsRunning)
            {
                localDb.StartInstance(instanceInfo.Name);
            }
        }
    }
}
