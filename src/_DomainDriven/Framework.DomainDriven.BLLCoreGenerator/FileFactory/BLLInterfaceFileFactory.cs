using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Projection;
using Framework.Security;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    /// <summary>
    /// Фабричный класс для BLL-интерфейсов к доменным объектам
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    public class BLLInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public BLLInterfaceFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {

        }


        public override FileType FileType => FileType.BLLInterface;


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration
            {
                Name = this.Name,

                Attributes = MemberAttributes.Public,
                IsPartial = true,
                IsInterface = true,

                BaseTypes =
                {
                    (this.DomainType.IsSecurity() ? typeof(IDefaultSecurityDomainBLLBase<,,,>) : typeof(IDefaultDomainBLLBase<,,,>))
                  .ToTypeReference(

                    this.Configuration.BLLContextInterfaceTypeReference,
                    this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                    this.DomainType.ToTypeReference(),
                    this.Configuration.Environment.GetIdentityType().ToTypeReference()),
                },
            };
        }

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
                            typeof(IFetchContainer<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
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
                            typeof(IFetchContainer<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
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
                            typeof(IFetchContainer<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
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
                        ReturnType = typeof(OData.SelectOperationResult<>).MakeGenericType(this.DomainType).ToTypeReference(),
                        Parameters =
                        {
                            typeof(OData.SelectOperation<>).ToTypeReference(this.DomainType).ToParameterDeclarationExpression("selectOperation"),
                            contextFilterModelType.ToTypeReference().ToParameterDeclarationExpression("filter"),
                            typeof(IFetchContainer<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
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
                        ReturnType = typeof(OData.SelectOperationResult<>).MakeGenericType(this.DomainType).ToTypeReference(),
                        Parameters =
                        {
                            typeof(OData.SelectOperation<>).ToTypeReference(this.DomainType).ToParameterDeclarationExpression("selectOperation"),
                            filterModelType.ToTypeReference().ToParameterDeclarationExpression("filter"),
                            typeof(IFetchContainer<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
                        }
                    };
                }

                if (this.DomainType.GetProjectionSourceTypeOrSelf().IsHierarchical())
                {
                    foreach (var filterModelType in this.DomainType.GetProjectionFilters(ProjectionFilterTargets.ODataTree))
                    {
                        yield return new CodeMemberMethod
                        {
                            Name = "GetTreeByOData",
                            ReturnType = typeof(OData.SelectOperationResult<>).MakeGenericType(typeof(HierarchicalNode<,>).MakeGenericType(this.DomainType, this.Configuration.Environment.GetIdentityType())).ToTypeReference(),
                            Parameters =
                            {
                                typeof(OData.SelectOperation<>).ToTypeReference(this.DomainType).ToParameterDeclarationExpression("selectOperation"),
                                filterModelType.ToTypeReference().ToParameterDeclarationExpression("filter"),
                                typeof(IFetchContainer<>).MakeGenericType(this.DomainType).ToTypeReference().ToParameterDeclarationExpression("fetchs")
                            }
                        };
                    }
                }
            }
        }
    }
}
