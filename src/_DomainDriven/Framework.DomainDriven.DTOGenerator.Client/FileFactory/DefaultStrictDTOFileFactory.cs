using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultStrictDTOFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, DTOFileType>, IClientMappingServiceExternalMethodGenerator
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultStrictDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.CodeTypeReferenceService = new StrictCodeTypeReferenceService<TConfiguration>(this.Configuration);
        }


        public override DTOFileType FileType { get; } = DTOGenerator.FileType.StrictDTO;


        protected override bool? InternalBaseTypeContainsPropertyChange { get; } = null;


        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }


        protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
        {
            if (!this.CodeTypeReferenceService.IsOptional(property))
            {
                if (property.PropertyType.IsCollection())
                {
                    return this.CodeTypeReferenceService.GetCodeTypeReference(property, true).ToObjectCreateExpression();
                }

                if (property.PropertyType == typeof(Period))
                {
                    return typeof(Period).ToTypeReferenceExpression().ToPropertyReference("Eternity");
                }
            }

            return base.GetFieldInitExpression(property);
        }

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

            if (this.IsPersistent())
            {
                yield return this.GetIdentityObjectTypeReference();

                if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
                {
                    yield return this.GetIdentityObjectContainerTypeReference();
                }
            }
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            if (this.IsPersistent())
            {
                yield return this.GetIdentityObjectImplementation(true);

                if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
                {
                    yield return this.GetIdentityObjectContainerImplementation();
                }
            }

            yield return this.GenerateDefaultConstructor();

            foreach (var ctor in this.GenerateStrictConstructors())
            {
                yield return ctor;
            }
        }

        public IEnumerable<CodeMemberMethod> GetClientMappingServiceMethods()
        {
            var propertyAssigner = new ClientMainToStrictPropertyAssigner<TConfiguration>(this).WithSecurityToSecurity(new MainCodeTypeReferenceService<TConfiguration>(this.Configuration));

            foreach (var sourceFileType in this.GetActualStrictConstructorFileTypes())
            {
                var targetParameter = this.CurrentReference.ToParameterDeclarationExpression("target");

                var currentSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, sourceFileType).ToParameterDeclarationExpression("currentSource");

                var properties = this.Configuration.GetDomainTypeProperties(this.DomainType, sourceFileType).Intersect(this.GetProperties(false));

                yield return new CodeMemberMethod
                {
                    Name = $"Map{sourceFileType.ShortName}To{this.FileType.ShortName}For{this.DomainType.Name}",
                    Attributes = MemberAttributes.Public,
                    Parameters = { targetParameter, currentSourceParameter }
                }.WithStatement(new CodeThrowArgumentNullExceptionConditionStatement(targetParameter))
                 .WithStatement(new CodeThrowArgumentNullExceptionConditionStatement(currentSourceParameter))
                 .WithStatements(properties.Select(property => propertyAssigner.GetAssignStatementBySource(property, currentSourceParameter.ToVariableReferenceExpression(), targetParameter.ToVariableReferenceExpression())));
            }
        }

        public IEnumerable<CodeMemberMethod> GetClientMappingServiceInterfaceMethods()
        {
            foreach (var sourceFileType in this.GetActualStrictConstructorFileTypes())
            {
                var currentSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, sourceFileType).ToParameterDeclarationExpression("currentSource");

                var targetParameter = this.CurrentReference.ToParameterDeclarationExpression("target");
                
                yield return new CodeMemberMethod
                {
                    Name = $"Map{sourceFileType.ShortName}To{this.FileType.ShortName}For{this.DomainType.Name}",
                    Parameters = { targetParameter, currentSourceParameter }
                };
            }
        }
    }
}
