using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;
using Framework.Persistent;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory
{
    /// <summary>
    /// Default updateDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultUpdateDTOFileFactory<TConfiguration> : PropertyFileFactory<TConfiguration, DTOFileType>, IClientMappingServiceExternalMethodGenerator
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultUpdateDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public override DTOFileType FileType => DTOGenerator.FileType.UpdateDTO;
        
        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService => new UpdateCodeTypeReferenceService<TConfiguration>(this.Configuration);

        protected override bool? InternalBaseTypeContainsPropertyChange => null;


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsClass = true,
                IsPartial = true,
                Attributes = MemberAttributes.Public
            };
        }

        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            yield return typeof(IUpdateDTO).ToTypeReference();
        }

        protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
        {
            return null;
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            if (this.IsPersistent())
            {
                if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
                {
                    yield return this.GetIdentityObjectContainerImplementation();
                }
            }

            {
                yield return this.CreateSourceFactory();

                var checkProperties = this.GetProperties(true).Where(prop => !prop.HasAttribute<VersionAttribute>()).ToList();

                yield return new CodeMemberProperty
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final,
                    Name = nameof(IUpdateDTO.IsEmpty),
                    Type = typeof(bool).ToTypeReference(),
                    HasGet = true,
                    GetStatements =
                    {
                        checkProperties.ToArray(this.GetPropertyIsEmptyCondition)
                            .Pipe(items => items.Any() ? (CodeExpression)new CodeBooleanAndOperatorExpression(items) : true.ToPrimitiveExpression())
                            .ToMethodReturnStatement()
                    }
                };

                yield return new CodeMemberMethod
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final,
                    Name = nameof(IUpdateDTO.Compress),
                    ReturnType = typeof(void).ToTypeReference()
                }.WithStatements(checkProperties.Select(this.TryGetPropertyCompressStatement).Where(statement => statement != null));
            }
        }

        private IEnumerable<MainDTOFileType> GetFileTypes()
        {
            yield return DTOGenerator.FileType.RichDTO;
            yield return DTOGenerator.FileType.FullDTO;
            yield return DTOGenerator.FileType.SimpleDTO;

            if (this.IsPersistent())
            {
                yield return DTOGenerator.FileType.BaseAuditPersistentDTO;
            }
        }

        public IEnumerable<CodeMemberMethod> GetClientMappingServiceMethods()
        {
            var targetParameter = this.CurrentReference.ToParameterDeclarationExpression("target");
            var targetParameterRefExpr = targetParameter.ToVariableReferenceExpression();

            var currentSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("currentSource");
            var currentSourceParameterRefExpr = currentSourceParameter.ToVariableReferenceExpression();

            var baseSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("baseSource");
            var baseSourceParameterRefExpr = baseSourceParameter.ToVariableReferenceExpression();

            {

                yield return new CodeMemberMethod
                {
                    Attributes = MemberAttributes.Public,
                    Name = $"mapUpdateFromStrictFor{this.DomainType.Name}",
                    Parameters = { targetParameter, currentSourceParameter, baseSourceParameter },
                    Statements =
                    {
                        new CodeConditionStatement
                        {
                            Condition = baseSourceParameterRefExpr,
                            TrueStatements = { new CodeThisReferenceExpression().ToMethodInvokeExpression($"mapUpdateFromPairStrictsFor{this.DomainType.Name}", targetParameterRefExpr, currentSourceParameterRefExpr, baseSourceParameterRefExpr) },
                            FalseStatements = { new CodeThisReferenceExpression().ToMethodInvokeExpression($"mapUpdateFromSingleStrictFor{this.DomainType.Name}", targetParameterRefExpr, currentSourceParameterRefExpr) }
                        }
                    }
                };
            }

            {
                var propertyAssigner = new UpdatePropertyAssigner<TConfiguration>(this);

                yield return new CodeMemberMethod
                {
                    Name = $"mapUpdateFromSingleStrictFor{this.DomainType.Name}",
                    Attributes = MemberAttributes.Private,
                    Parameters = { targetParameter, currentSourceParameter }
                }.WithStatements(this.GetProperties(false).Select(property => propertyAssigner.GetAssignStatement(property, currentSourceParameterRefExpr.ToPropertyReference(property), targetParameterRefExpr.ToPropertyReference(property))));
            }

            {
                var propertyAssigner = new DiffUpdatePropertyAssigner<TConfiguration>(this);

                yield return new CodeMemberMethod
                {
                    Name = $"mapUpdateFromPairStrictsFor{this.DomainType.Name}",
                    Attributes = MemberAttributes.Private,
                    Parameters = { targetParameter, currentSourceParameter, baseSourceParameter }
                }.WithStatements(this.GetProperties(false).Select(property => propertyAssigner.GetAssignStatement(property, baseSourceParameterRefExpr.ToPropertyReference(property), currentSourceParameterRefExpr.ToPropertyReference(property), targetParameterRefExpr.ToPropertyReference(property))))
                 .WithStatement(targetParameterRefExpr.ToMethodInvokeExpression("compress").ToExpressionStatement());
            }
        }

        public IEnumerable<CodeMemberMethod> GetClientMappingServiceInterfaceMethods()
        {
            {
                var targetParameter = this.CurrentReference.ToParameterDeclarationExpression("target");

                var currentSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("currentSource");
                
                var baseSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("baseSource");

                yield return new CodeMemberMethod
                {
                    Name = $"mapUpdateFromStrictFor{this.DomainType.Name}",
                    Parameters = { targetParameter, currentSourceParameter, baseSourceParameter }
                };
            }
        }

        private CodeMemberMethod CreateSourceFactory()
        {
            var currentSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("source");

            var baseSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("baseSource");//.WithOptional();

            var mappingServiceParameter = this.Configuration.GetCodeTypeReference(null, DTOGenerator.FileType.ClientDTOMappingServiceInterface).ToParameterDeclarationExpression("mappingService");//.WithOptional();
            var mappingServiceParameterRefExpr = mappingServiceParameter.ToVariableReferenceExpression();


            var initMappingServiceStatement = new CodeConditionStatement
            {
                Condition = mappingServiceParameter.ToVariableReferenceExpression().ToIsNullOrUndefinedExpression(),
                TrueStatements =
                {
                    this.Configuration.GetDefaultClientDTOMappingServiceExpression().ToAssignStatement(mappingServiceParameterRefExpr)
                }
            };

            var updateDTOVariableStatement = this.CurrentReference.ToVariableDeclarationStatement("result", this.CurrentReference.ToObjectCreateExpression());
            var updateDTOVariable = updateDTOVariableStatement.ToVariableReferenceExpression();

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = "fromStrict",
                Parameters = { currentSourceParameter, baseSourceParameter, mappingServiceParameter },
                ReturnType = this.CurrentReference,
                Statements =
                {
                    initMappingServiceStatement,
                    updateDTOVariableStatement,
                    mappingServiceParameterRefExpr.ToMethodInvokeExpression(
                        $"mapUpdateFromStrictFor{this.DomainType.Name}",
                        updateDTOVariable,
                        currentSourceParameter.ToVariableReferenceExpression(),
                        baseSourceParameter.ToVariableReferenceExpression()),
                    updateDTOVariable.ToMethodReturnStatement()
                }
            };
        }

        private CodeExpression GetPropertyIsEmptyCondition(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var propRef = new CodeThisReferenceExpression().ToPropertyReference(property);

            if (this.Configuration.IsCollectionProperty(property))
            {
                return new CodeValueEqualityOperatorExpression(propRef.ToPropertyReference(nameof(Array.Length).ToStartLowerCase()), 0.ToPrimitiveExpression());
            }
            else
            {
                return new CodeValueEqualityOperatorExpression(new CodeSnippetExpression($"typeof this.{propRef.PropertyName}"), "undefined".ToPrimitiveExpression());
            }
        }

        private CodeStatement TryGetPropertyCompressStatement(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var propRef = new CodeThisReferenceExpression().ToPropertyReference(property);

            if (this.Configuration.IsCollectionProperty(property))
            {
                var elementType = property.PropertyType.GetCollectionElementType();

                var elementUpdateTypeRef = this.Configuration.GetCodeTypeReference(elementType, DTOType.UpdateDTO);
                var elementIdentityTypeRef = this.Configuration.GetCodeTypeReference(elementType, DTOType.IdentityDTO);

                return typeof(UpdateExtensions).ToTypeReferenceExpression()
                                               .ToMethodReferenceExpression(nameof(UpdateExtensions.Compress), elementUpdateTypeRef, elementIdentityTypeRef)
                                               .ToMethodInvokeExpression(propRef)
                                               .ToExpressionStatement();
            }
            else if (this.Configuration.IsReferenceProperty(property) && property.IsDetail())
            {
                var getActualElementExpr = typeof(UpdateExtensions).ToTypeReferenceExpression()
                                                                   .ToMethodReferenceExpression(nameof(UpdateExtensions.GetActualUpdateElement))
                                                                   .ToMethodInvokeExpression(propRef);

                return getActualElementExpr.ToAssignStatement(propRef);
            }

            else
            {
                return null;
            }
        }
    }
}
