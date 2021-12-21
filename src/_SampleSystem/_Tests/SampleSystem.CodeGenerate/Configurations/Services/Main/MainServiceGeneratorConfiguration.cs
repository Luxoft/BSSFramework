using System;
using System.Collections.Generic;

using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.Projection;

namespace SampleSystem.CodeGenerate
{
    /// <summary>
    /// Конфигурация для генерации основного фасада SampleSystem-системы (пример с добавлением генерации фасадных методов по ComplexChange-модели)
    /// </summary>
    public class MainServiceGeneratorConfiguration : MainGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public MainServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override IGeneratePolicy<MethodIdentity> GeneratePolicy { get; } = new CustomServiceGeneratePolicy();


        public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
        {
            foreach (var method in base.GetMethodGenerators(domainType))
            {
                yield return method;
            }

            if (!domainType.IsProjection())
            {
                foreach (var complexChangeModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.ComplexChangeModelType))
                {
                    yield return new ComplexChangeMethodGenerator(this, domainType, complexChangeModelType);
                }
            }
        }
    }
}
