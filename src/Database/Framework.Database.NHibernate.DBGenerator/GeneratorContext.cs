namespace Framework.Database.NHibernate.DBGenerator;

internal class GeneratorContext(ModifyMode modifyMode, MergeColumnMode mergeColumnMode)
{
    public ModifyMode ModifyMode => modifyMode;

    public MergeColumnMode MergeColumnMode => mergeColumnMode;
}
