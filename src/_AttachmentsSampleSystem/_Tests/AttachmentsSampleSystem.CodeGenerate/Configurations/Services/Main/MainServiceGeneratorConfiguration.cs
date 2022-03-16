using Framework.DomainDriven.ServiceModelGenerator;

namespace AttachmentsSampleSystem.CodeGenerate
{
    /// <summary>
    /// Конфигурация для генерации основного фасада AttachmentsSampleSystem-системы (пример с добавлением генерации фасадных методов по ComplexChange-модели)
    /// </summary>
    public class MainServiceGeneratorConfiguration : MainGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public MainServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}
