namespace Framework.Database.Attr;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DbSessionModeAttribute(DBSessionMode sessionMode) : Attribute
{
    public DBSessionMode SessionMode { get; } = sessionMode;
}
