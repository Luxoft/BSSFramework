namespace Automation.Utils.DatabaseUtils.Interfaces
{
    public interface IDatabaseUtil
    {
        public IDatabaseContext DatabaseContext { get; }
        void CreateDatabase();
        void DropDatabase();
        void ExecuteInsertsForDatabases();
        void DropAllDatabases();
        void CopyDetachedFiles();
        void AttachDatabase();
        void GenerateDatabases();
        void DeleteDetachedFiles();
        void CheckAndCreateDetachedFiles();
        void CheckTestDatabase();
        void CheckServerAllowed();
        void GenerateTestData();
    }
}