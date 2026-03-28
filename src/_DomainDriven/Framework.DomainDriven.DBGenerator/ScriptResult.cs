namespace Framework.DomainDriven.DBGenerator;

public class ScriptResult
{
    private readonly SmoObjectActionType _smoObjectActionType;
    private readonly List<string> _scripts;

    public ScriptResult(SmoObjectActionType smoObjectActionType, List<string> scripts)
    {
        if (scripts == null) throw new ArgumentNullException(nameof(scripts));
        this._smoObjectActionType = smoObjectActionType;
        this._scripts = scripts;
    }

    public SmoObjectActionType SMOObjectActionType
    {
        get { return this._smoObjectActionType; }
    }

    public List<string> Scripts
    {
        get { return this._scripts; }
    }
}
