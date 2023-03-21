namespace Framework.DomainDriven.DBGenerator.Team;

public enum ApplyMigrationDbScriptMode
{
    PreAddOrUpdate = 0,
    AddOrUpdate = 1,
    PostAddOrUpdate = 2,
    PreChangeIndexies = 3,
    ChangeIndexies = 4,
    PostChangeIndexies = 5,
    PreRemove = 6,
    Remove = 7,
    PostRemove = 8,
    ChangeDefaultValue = 9,
}
