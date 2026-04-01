using System.CodeDom;

using CommonFramework;

using Framework.BLL;
using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.Models;
using Framework.BLL.Domain.Persistent;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.FileFactory.__Base;
using Framework.Core;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

using GenericQueryable.Fetching;

using OData.Domain;

namespace Framework.CodeGeneration.BLLCoreGenerator.FileFactory;

/// <summary>
/// Фабричный класс для BLL-интерфейсов к доменным объектам
/// </summary>
/// <typeparam name="TConfiguration"></typeparam>
public class BLLInterfaceFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : FileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment>
{
    public override FileType FileType => FileType.BLLInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Name,

            Attributes = MemberAttributes.Public,
            IsPartial = true,
            IsInterface = true,

            BaseTypes =
            {
                typeof(IDefaultSecurityDomainBLLBase<,,,>)
                    .ToTypeReference(

                        this.Configuration.BLLContextInterfaceTypeReference,
                        this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                        this.DomainType.ToTypeReference(),
                        this.Configuration.Environment.GetIdentityType().ToTypeReference()),
            },
        };

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        {
            foreach (var createModelType in this.Configuration.Environment.GetModelTypes(this.DomainType, this.Configuration.CreateModelType))
            {
                createModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new CodeMemberMethod
                             {
                                     Name = this.DomainType.GetModelMethodName(createModelType, ModelRole.Create, false),
                                     ReturnType = this.DomainType.ToTypeReference(),
                                     Parameters =
                                     {
                                             createModelType.ToTypeReference().ToParameterDeclarationExpression("createModel")
                                     }
                             };
            }
        }

        {
            foreach (var changeModelType in this.Configuration.Environment.GetModelTypes(this.DomainType, this.Configuration.ChangeModelType))
            {
                var directMode = changeModelType.GetDirectMode();
                var methodName = this.DomainType.GetModelMethodName(changeModelType, ModelRole.Change, false);

                if (directMode.HasFlag(DirectMode.Out))
                {
                    yield return new CodeMemberMethod
                                 {
                                         Name = "Get" + methodName,
                                         ReturnType = changeModelType.ToTypeReference(),
                                         Parameters =
                                         {
                                                 this.DomainType.ToTypeReference().ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase())
                                         }
                                 };
                }

                if (directMode.HasFlag(DirectMode.In))
                {
                    yield return new CodeMemberMethod
                                 {
                                         Name = methodName,
                                         ReturnType = this.DomainType.ToTypeReference(),
                                         Parameters =
                                         {
                                                 changeModelType.ToTypeReference().ToParameterDeclarationExpression("changeModel")
                                         }
                                 };
                }
            }
        }

        {
            foreach (var changeModelType in this.Configuration.Environment.GetModelTypes(this.DomainType, this.Configuration.MassChangeModelType))
            {
                var directMode = changeModelType.GetDirectMode();
                var methodName = this.DomainType.GetModelMethodName(changeModelType, ModelRole.MassChange, false);

                if (directMode.HasFlag(DirectMode.Out))
                {
                    yield return new CodeMemberMethod
                                 {
                                         Name = "Get" + methodName,
                                         ReturnType = changeModelType.ToTypeReference(),
                                         Parameters =
                                         {
                                                 typeof(List<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression(this.DomainType.Name.ToPluralize().ToStartLowerCase())
                                         }
                                 };
                }

                if (directMode.HasFlag(DirectMode.In))
                {
                    yield return new CodeMemberMethod
                                 {
                                         Name = methodName,
                                         ReturnType = typeof(List<>).MakeGenericType(this.DomainType).ToTypeReference(),
                                         Parameters =
                                         {
                                                 changeModelType.ToTypeReference().ToParameterDeclarationExpression("changeModel")
                                         }
                                 };
                }
            }
        }

        {
            foreach (var contextFilterModelType in this.Configuration.Environment.GetModelTypes(this.DomainType, this.Configuration.ContextFilterModelType))
            {
                contextFilterModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new CodeMemberMethod
                             {
                                     Name = "GetListBy",
                                     ReturnType = typeof(List<>).MakeGenericType(this.DomainType).ToTypeReference(),
                                     Parameters =
                                     {
                                             contextFilterModelType.ToTypeReference().ToParameterDeclarationExpression("filter"),
                                             typeof(FetchRule<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
                                     }
                             };
            }
        }


        if (this.DomainType.IsProjection())
        {
            foreach (var contextFilterModelType in this.DomainType.GetProjectionFilters(ProjectionFilterTargets.Collection))
            {
                contextFilterModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new CodeMemberMethod
                             {
                                     Name = "GetListBy",
                                     ReturnType = typeof(List<>).MakeGenericType(this.DomainType).ToTypeReference(),
                                     Parameters =
                                     {
                                             contextFilterModelType.ToTypeReference().ToParameterDeclarationExpression("filter"),
                                             typeof(FetchRule<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
                                     }
                             };
            }

            foreach (var contextFilterModelType in this.DomainType.GetProjectionFilters(ProjectionFilterTargets.Single))
            {
                contextFilterModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new CodeMemberMethod
                             {
                                     Name = "GetObjectBy",
                                     ReturnType = this.DomainType.ToTypeReference(),
                                     Parameters =
                                     {
                                             contextFilterModelType.ToTypeReference().ToParameterDeclarationExpression("filter"),
                                             typeof(FetchRule<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
                                     }
                             };
            }
        }

        {
            foreach (var contextFilterModelType in this.Configuration.Environment.GetModelTypes(this.DomainType, this.Configuration.ODataContextFilterModelType))
            {
                contextFilterModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new CodeMemberMethod
                             {
                                     Name = "GetObjectsByOData",
                                     ReturnType = typeof(SelectOperationResult<>).MakeGenericType(this.DomainType).ToTypeReference(),
                                     Parameters =
                                     {
                                             typeof(SelectOperation<>).ToTypeReference(this.DomainType).ToParameterDeclarationExpression("selectOperation"),
                                             contextFilterModelType.ToTypeReference().ToParameterDeclarationExpression("filter"),
                                             typeof(FetchRule<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
                                     }
                             };
            }
        }

        {
            foreach (var extendedModelType in this.Configuration.Environment.GetModelTypes(this.DomainType, this.Configuration.ExtendedModelType))
            {
                var directMode = extendedModelType.GetDirectMode();
                var methodName = this.DomainType.GetModelMethodName(extendedModelType, ModelRole.Extended, false);

                if (directMode.HasFlag(DirectMode.Out))
                {
                    yield return new CodeMemberMethod
                                 {
                                         Name = "Get" + methodName,
                                         ReturnType = extendedModelType.ToTypeReference(),
                                         Parameters =
                                         {
                                                 this.DomainType.ToTypeReference().ToParameterDeclarationExpression("extendedModel")
                                         }
                                 };
                }

                if (directMode.HasFlag(DirectMode.In))
                {
                    yield return new CodeMemberMethod
                                 {
                                         Name = "Save" + methodName,
                                         ReturnType = this.DomainType.ToTypeReference(),
                                         Parameters =
                                         {
                                                 extendedModelType.ToTypeReference().ToParameterDeclarationExpression("extendedModel")
                                         }
                                 };
                }
            }
        }


        {
            foreach (var formatModelType in this.Configuration.Environment.GetModelTypes(this.DomainType, this.Configuration.FormatModelType))
            {
                formatModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new CodeMemberMethod
                             {
                                     Name = this.DomainType.GetModelMethodName(formatModelType, ModelRole.Format, false),
                                     ReturnType = this.DomainType.ToTypeReference(),
                                     Parameters =
                                     {
                                             formatModelType.ToTypeReference().ToParameterDeclarationExpression("formatModel")
                                     }
                             };
            }
        }

        {
            foreach (var integrationModelType in this.Configuration.Environment.GetModelTypes(this.DomainType, this.Configuration.IntegrationSaveModelType))
            {
                integrationModelType.CheckDirectMode(DirectMode.In, true);

                yield return new CodeMemberMethod
                             {
                                     Name = this.Configuration.IntegrationSaveMethodName,
                                     ReturnType = this.DomainType.ToTypeReference(),
                                     Parameters =
                                     {
                                             integrationModelType.ToTypeReference().ToParameterDeclarationExpression("integrationSaveModel")
                                     }
                             };
            }
        }

        if (this.DomainType.IsProjection())
        {
            foreach (var filterModelType in this.DomainType.GetProjectionFilters(ProjectionFilterTargets.OData))
            {
                yield return new CodeMemberMethod
                             {
                                     Name = "GetObjectsByOData",
                                     ReturnType = typeof(SelectOperationResult<>).MakeGenericType(this.DomainType).ToTypeReference(),
                                     Parameters =
                                     {
                                             typeof(SelectOperation<>).ToTypeReference(this.DomainType).ToParameterDeclarationExpression("selectOperation"),
                                             filterModelType.ToTypeReference().ToParameterDeclarationExpression("filter"),
                                             typeof(FetchRule<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
                                     }
                             };
            }

            if (this.Configuration.Environment.IsHierarchical(this.DomainType.GetProjectionSourceTypeOrSelf()))
            {
                foreach (var filterModelType in this.DomainType.GetProjectionFilters(ProjectionFilterTargets.ODataTree))
                {
                    yield return new CodeMemberMethod
                                 {
                                         Name = "GetTreeByOData",
                                         ReturnType = typeof(SelectOperationResult<>).MakeGenericType(typeof(HierarchicalNode<,>).MakeGenericType(this.DomainType, this.Configuration.Environment.GetIdentityType())).ToTypeReference(),
                                         Parameters =
                                         {
                                                 typeof(SelectOperation<>).ToTypeReference(this.DomainType).ToParameterDeclarationExpression("selectOperation"),
                                                 filterModelType.ToTypeReference().ToParameterDeclarationExpression("filter"),
                                                 typeof(FetchRule<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
                                         }
                                 };
                }
            }
        }
    }
}
