namespace Framework.Database.NHibernate.DBGenerator;

public class ScriptResult
{
    private readonly SmoObjectActionType smoObjectActionType;
    private readonly List<string> scripts;

    public ScriptResult(SmoObjectActionType smoObjectActionType, List<string> scripts)
    {
        if (scripts == null) throw new ArgumentNullException(nameof(scripts));
        this.smoObjectActionType = smoObjectActionType;
        this.scripts = scripts;
    }

    public SmoObjectActionType SmoObjectActionType
    {
        get { return this.smoObjectActionType; }
    }

    public List<string> Scripts
    {
        get { return this.scripts; }
    }
}
