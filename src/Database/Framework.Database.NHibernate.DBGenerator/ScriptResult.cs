namespace Framework.Database.NHibernate.DBGenerator;

public class ScriptResult
{
    public ScriptResult(SmoObjectActionType smoObjectActionType, List<string> scripts)
    {
        if (scripts == null) throw new ArgumentNullException(nameof(scripts));
        this.SmoObjectActionType = smoObjectActionType;
        this.Scripts = scripts;
    }

    public SmoObjectActionType SmoObjectActionType { get; }

    public List<string> Scripts { get; }
}
