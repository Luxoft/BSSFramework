namespace Framework.DomainDriven.DBGenerator;

internal class GeneratorContext
{
    private readonly ModifyMode _modifyMode;
    private readonly MergeColumnMode _mergeColumnMode;

    public GeneratorContext(ModifyMode modifyMode, MergeColumnMode mergeColumnMode)
    {
        this._modifyMode = modifyMode;
        this._mergeColumnMode = mergeColumnMode;
    }

    public ModifyMode ModifyMode
    {
        get { return this._modifyMode; }
    }

    public MergeColumnMode MergeColumnMode
    {
        get { return this._mergeColumnMode; }
    }
}
