namespace Framework.DomainDriven;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DBSessionModeAttribute(DBSessionMode sessionMode) : Attribute
{
    public DBSessionMode SessionMode { get; } = sessionMode;
}
