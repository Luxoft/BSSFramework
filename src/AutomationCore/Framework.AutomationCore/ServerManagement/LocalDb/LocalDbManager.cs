//using MartinCostello.SqlLocalDb;

//namespace Framework.AutomationCore.ServerManagement.LocalDb;

//public class LocalDbManager
//{
//    public void CreateLocalDb(string instanceName)
//    {
//        using var localDb = new SqlLocalDbApi();
//        var instanceInfo = localDb.GetInstanceInfo(instanceName);
//        localDb.AutomaticallyDeleteInstanceFiles = true;

//        if (!instanceInfo.Exists)
//        {
//            localDb.CreateInstance(instanceInfo.Name);
//        }

//        if (!instanceInfo.IsRunning)
//        {
//            localDb.StartInstance(instanceInfo.Name);
//        }
//    }

//    public static bool LocalDbInstanceExists(string instanceName)
//    {
//        using var localDbApi = new SqlLocalDbApi();
//        var instanceInfo = localDbApi.GetInstanceInfo(instanceName);

//        return instanceInfo.Exists;
//    }

//    public static void DeleteLocalDb(string instanceName, StopInstanceOptions stopOptions = StopInstanceOptions.KillProcess)
//    {
//        using var localDb = new SqlLocalDbApi();
//        var instanceInfo = localDb.GetInstanceInfo(instanceName);

//        if (instanceInfo.IsRunning)
//        {
//            localDb.StopInstance(instanceInfo.Name, stopOptions, TimeSpan.FromSeconds(1));
//        }

//        localDb.DeleteInstance(instanceName);
//    }
//}
