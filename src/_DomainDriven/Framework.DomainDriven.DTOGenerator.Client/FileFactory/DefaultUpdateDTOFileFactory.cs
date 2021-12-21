using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.CodeDom;
using Framework.Persistent;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultUpdateDTOFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, DTOFileType>, IClientMappingServiceExternalMethodGenerator
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultUpdateDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.CodeTypeReferenceService = new UpdateCodeTypeReferenceService<TConfiguration>(this.Configuration);
        }


        public override DTOFileType FileType { get; } = DTOGenerator.FileType.UpdateDTO;


        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }


        protected override bool? InternalBaseTypeContainsPropertyChange { get; } = null;


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
                {
                    IsClass = true,
                    IsPartial = true,
                    Attributes = MemberAttributes.Public,
                    BaseTypes = { typeof(IUpdateDTO).ToTypeReference() }
                };
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            if (this.IsPersistent())
            {
                yield return this.GetIdentityObjectTypeRef();
            }
        }

        protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
        {
            var fieldTypeRef = this.CodeTypeReferenceService.GetCodeTypeReference(property, true);

            return this.CodeTypeReferenceService.IsOptional(property) ? this.CodeTypeReferenceService.GetCodeTypeReference(property).ToNothingValueExpression()
                   : property.PropertyType.IsCollection() ? (CodeExpression)new CodeObjectCreateExpression(fieldTypeRef)
                   : property.GetCustomAttribute<DefaultValueAttribute>().Maybe(attr => attr.Value.ToDynamicPrimitiveExpression());
        }

        protected override IEnumerable<CodeConstructor> GetConstructors()
        {
            foreach (var ctor in base.GetConstructors())
            {
                yield return ctor;
            }

            yield return this.GenerateDefaultConstructor();

            yield return this.GenerateUpdateFromStrictConstructor(false);
            yield return this.GenerateUpdateFromStrictConstructor(true);
            yield return this.GenerateUpdateFromDiffStrictConstructor(false);
            yield return this.GenerateUpdateFromDiffStrictConstructor(true);
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            if (this.IsPersistent())
            {
                yield return this.GetIdentityObjectImplementation(true);
            }

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

        private CodeExpression GetPropertyIsEmptyCondition(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var propRef = new CodeThisReferenceExpression().ToPropertyReference(property);

            if (this.Configuration.IsCollectionProperty(property))
            {
                return new CodeValueEqualityOperatorExpression(propRef.ToPropertyReference(nameof(List<object>.Count)), 0.ToPrimitiveExpression());
            }
            else
            {
                return propRef.ToPropertyReference(nameof(IMaybe.HasValue)).ToNegateExpression();
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

        public IEnumerable<CodeMemberMethod> GetClientMappingServiceMethods()
        {
            var targetParameter = this.CurrentReference.ToParameterDeclarationExpression("target");
            var targetParameterRefExpr = targetParameter.ToVariableReferenceExpression();

            var currentSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("currentSource");
            var currentSourceParameterRefExpr = currentSourceParameter.ToVariableReferenceExpression();

            {
                var propertyAssigner = new UpdatePropertyAssigner<TConfiguration>(this);

                yield return new CodeMemberMethod
                {
                    Name = $"Map{this.DomainType.Name}",
                    Attributes = MemberAttributes.Public,
                    Parameters = { targetParameter, currentSourceParameter  }
                }.WithStatement(new CodeThrowArgumentNullExceptionConditionStatement(targetParameter))
                 .WithStatement(new CodeThrowArgumentNullExceptionConditionStatement(currentSourceParameter))
                 .WithStatements(this.GetProperties(false).Select(property => propertyAssigner.GetAssignStatement(property, currentSourceParameterRefExpr.ToPropertyReference(property), targetParameterRefExpr.ToPropertyReference(property))));
            }

            {
                var baseSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("baseSource");
                var baseSourceParameterRefExpr = baseSourceParameter.ToVariableReferenceExpression();

                var propertyAssigner = new DiffUpdatePropertyAssigner<TConfiguration>(this);

                yield return new CodeMemberMethod
                {
                    Name = $"Map{this.DomainType.Name}",
                    Attributes = MemberAttributes.Public,
                    Parameters = { targetParameter, currentSourceParameter, baseSourceParameter }
                }.WithStatement(new CodeThrowArgumentNullExceptionConditionStatement(targetParameter))
                 .WithStatement(new CodeThrowArgumentNullExceptionConditionStatement(currentSourceParameter))
                 .WithStatement(new CodeThrowArgumentNullExceptionConditionStatement(baseSourceParameter))
                 .WithStatements(this.GetProperties(false).Select(property => propertyAssigner.GetAssignStatement(property, baseSourceParameterRefExpr.ToPropertyReference(property), currentSourceParameterRefExpr.ToPropertyReference(property), targetParameterRefExpr.ToPropertyReference(property))))
                 .WithStatement(targetParameterRefExpr.ToMethodInvokeExpression(nameof(IUpdateDTO.Compress)).ToExpressionStatement());
            }
        }

        public IEnumerable<CodeMemberMethod> GetClientMappingServiceInterfaceMethods()
        {
            var targetParameter = this.CurrentReference.ToParameterDeclarationExpression("target");

            var currentSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("currentSource");
            
            {
                yield return new CodeMemberMethod
                {
                    Name = $"Map{this.DomainType.Name}",
                    Parameters = { targetParameter, currentSourceParameter }
                };
            }

            {
                var baseSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.StrictDTO).ToParameterDeclarationExpression("baseSource");
                
                yield return new CodeMemberMethod
                {
                    Name = $"Map{this.DomainType.Name}",
                    Parameters = { targetParameter, currentSourceParameter, baseSourceParameter }
                };
            }
        }
    }
}
