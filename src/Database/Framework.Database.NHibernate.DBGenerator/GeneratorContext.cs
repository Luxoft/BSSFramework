namespace Framework.Database.NHibernate.DBGenerator;

internal class GeneratorContext(ModifyMode modifyMode, MergeColumnMode mergeColumnMode)
{
    public ModifyMode ModifyMode
    {
        get { return modifyMode; }
    }

    public MergeColumnMode MergeColumnMode
    {
        get { return mergeColumnMode; }
    }
}
