using Framework.DomainDriven.Metadata;

namespace Framework.DomainDriven
{
    public interface IDBGenerator
    {
        void Generate(AssemblyMetadata metadata, DBGeneratorParameters parameters);
        void Generate(AssemblyMetadata metadata, DBGeneratorParameters parameters, ModifyMode modifyTableMode);
        void Generate(AssemblyMetadata metadata, DBGeneratorParameters parameters, MergeColumnMode mergeColumnMode);

        void Generate(AssemblyMetadata metadata, DBGeneratorParameters parameters, ModifyMode modifyTableMode,
                      MergeColumnMode mergeColumnMode);

    }
}