using System.Collections.Immutable;

namespace Framework.CodeGeneration.Configuration;

public interface ICodeGenerationEnvironment : FileGeneration.Configuration.IFileGenerationEnvironment
{
    ImmutableArray<Type> SecurityRuleTypeList { get; }

    bool IsHierarchical(Type type);
}
