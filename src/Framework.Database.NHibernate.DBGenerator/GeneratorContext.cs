namespace Framework.Database.NHibernate.DBGenerator;

internal class GeneratorContext
{
    private readonly ModifyMode modifyMode;
    private readonly MergeColumnMode mergeColumnMode;

    public GeneratorContext(ModifyMode modifyMode, MergeColumnMode mergeColumnMode)
    {
        this.modifyMode = modifyMode;
        this.mergeColumnMode = mergeColumnMode;
    }

    public ModifyMode ModifyMode
    {
        get { return this.modifyMode; }
    }

    public MergeColumnMode MergeColumnMode
    {
        get { return this.mergeColumnMode; }
    }
}
