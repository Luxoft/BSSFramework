namespace Framework.DomainDriven.Generation.Domain;

public interface IGeneratorConfiguration : IRenderingConfiguration
{
    IReadOnlyCollection<Type> DomainTypes { get; }
}

public interface IGeneratorConfiguration<out TEnvironment> : IGeneratorConfiguration
        where TEnvironment : IGenerationEnvironment
{
    TEnvironment Environment { get; }
}

public interface IGeneratorConfiguration<out TEnvironment, in TFileType> : IGeneratorConfiguration<TEnvironment>, ICodeTypeReferenceService<TFileType>
        where TEnvironment : IGenerationEnvironment
{
}
