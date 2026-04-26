using Anch.Core;

using Framework.Application.Domain;
using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.Persistent.IdentityObject;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Query;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.Remove;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.Save;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Main;

/// <summary>
/// Конфигурация для генерации основного фасада системы
/// </summary>
/// <typeparam name="TEnvironment"></typeparam>
public abstract class MainGeneratorConfigurationBase<TEnvironment>(TEnvironment environment)
    : ServiceModelGeneratorBase<TEnvironment>(environment), IMainGeneratorConfiguration<TEnvironment>
    where TEnvironment : class, IServiceModelGenerationEnvironment
{
    public override string ImplementClassName { get; } = "Facade";

    protected override string NamespacePostfix { get; } = "ServiceFacade";

    public virtual bool GenerateQueryMethods { get; } = false;

    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        foreach (var dtoType in this.Environment.MetadataProxyProvider.Wrap(domainType).GetViewDTOTypes())
        {
            if (domainType.IsInterfaceImplementation(typeof(IVisualIdentityObject)))
            {
                yield return new GetSingleByNameMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);
            }

            if (domainType.IsInterfaceImplementation(typeof(ICodeObject<>)))
            {
                yield return new GetSingleByCodeMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);
            }

            yield return new GetSingleByIdentityMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);

            yield return new GetListByIdentsMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);
            yield return new GetListMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);

            foreach (var filterModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.FilterModelType))
            {
                filterModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new GetListByFilterModelMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType, filterModelType);
            }

            foreach (var filterModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.ContextFilterModelType))
            {
                filterModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new GetListByFilterModelMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType, filterModelType);
            }

            if (domainType.IsProjection())
            {
                foreach (var filterModelType in domainType.GetProjectionFilters(ProjectionFilterTargets.Collection))
                {
                    yield return new GetListByFilterModelMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType, filterModelType);
                }

                foreach (var filterModelType in domainType.GetProjectionFilters(ProjectionFilterTargets.Single))
                {
                    yield return new GetSingleByFilterModelMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType, filterModelType);
                }
            }

            if (this.GenerateQueryMethods)
            {
                foreach (var methodGenerator in new QueryServiceMethodGeneratorCollection<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType))
                {
                    yield return methodGenerator;
                }
            }

            yield return new GetListByOperationMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);

            if (this.Environment.IsHierarchical(domainType))
            {
                yield return new GetTreeByOperationMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);
            }
        }


        if (!domainType.IsProjection())
        {
            yield return new HasAccessMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType);
            yield return new CheckAccessMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType);

            {
                var singleSaveGenerator = new SaveMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType);

                yield return singleSaveGenerator;

                yield return new SaveManyMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(singleSaveGenerator);

                yield return new UpdateMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType);
            }

            {
                foreach (var formatModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.FormatModelType))
                {
                    formatModelType.CheckDirectMode(DirectMode.Out, true);

                    yield return new FormatMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, formatModelType);
                }
            }

            {
                foreach (var createModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.CreateModelType))
                {
                    createModelType.CheckDirectMode(DirectMode.Out, true);

                    yield return new CreateMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, createModelType);
                }
            }

            {
                foreach (var changeModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.ChangeModelType))
                {
                    yield return new GetChangeModelMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, changeModelType);

                    yield return new ChangeMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, changeModelType);
                }
            }

            {
                foreach (var massChangeModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.MassChangeModelType))
                {
                    yield return new GetMassChangeModelMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, massChangeModelType);

                    yield return new MassChangeMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, massChangeModelType);
                }
            }

            {
                var singleRemoveGenerator = new RemoveMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType);

                yield return singleRemoveGenerator;

                yield return new RemoveManyMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(singleRemoveGenerator);
            }

            {
                foreach (var extendedModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.ExtendedModelType))
                {
                    yield return new GetExtendedModelMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, extendedModelType);

                    yield return new SaveExtendedMethodGenerator<MainGeneratorConfigurationBase<TEnvironment>>(this, domainType, extendedModelType);
                }
            }
        }
    }
}
