namespace Framework.Database.NHibernate.DBGenerator;

public class ScriptResult(SmoObjectActionType smoObjectActionType, List<string> scripts)
{
    public SmoObjectActionType SmoObjectActionType { get; } = smoObjectActionType;

    public List<string> Scripts { get; } = scripts ?? throw new ArgumentNullException(nameof(scripts));
}
