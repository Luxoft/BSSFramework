namespace Framework.Application.Session.Attr;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DbSessionModeAttribute(DBSessionMode sessionMode) : Attribute
{
    public DBSessionMode SessionMode { get; } = sessionMode;
}
