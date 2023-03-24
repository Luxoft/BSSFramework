using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DBGenerator;

public class ScriptResult
{
    private readonly SmoObjectActionType _smoObjectActionType;
    private readonly IList<string> _scripts;

    public ScriptResult(SmoObjectActionType smoObjectActionType, [NotNull] IList<string> scripts)
    {
        if (scripts == null) throw new ArgumentNullException(nameof(scripts));
        this._smoObjectActionType = smoObjectActionType;
        this._scripts = scripts;
    }

    public SmoObjectActionType SMOObjectActionType
    {
        get { return this._smoObjectActionType; }
    }

    public IList<string> Scripts
    {
        get { return this._scripts; }
    }
}
