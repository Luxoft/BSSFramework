using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.TypeScript
{
    public class TypeScriptClientPrimitiveDTOMappingServiceFileFactory<TConfiguration> : FileFactory<TConfiguration, FileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        private readonly ReadOnlyCollection<IClientMappingServiceExternalMethodGenerator> _externalGenerators;

        public TypeScriptClientPrimitiveDTOMappingServiceFileFactory(TConfiguration configuration, IEnumerable<IClientMappingServiceExternalMethodGenerator> externalGenerators)
            : base(configuration, null)
        {
            if (externalGenerators == null) throw new ArgumentNullException(nameof(externalGenerators));

            this._externalGenerators = externalGenerators.ToReadOnlyCollection();
        }


        public override FileType FileType { get; } = FileType.ClientPrimitiveDTOMappingService;

        public override CodeTypeReference BaseReference => typeof(ClientDTOMappingServiceBase).ToTypeReference();


        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            yield return this.Configuration.GetCodeTypeReference(null, FileType.ClientDTOMappingServiceInterface);
        }

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                Attributes = MemberAttributes.Abstract,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Abstract,
                IsPartial = true,
            };
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var member in base.GetMembers())
            {
                yield return member;
            }


            foreach (var fieldFileFactory in this._externalGenerators)
            {
                foreach (var method in fieldFileFactory.GetClientMappingServiceMethods())
                {
                    yield return method;
                }
            }

            var currentInstanceField = new CodeMemberField
            {
                Attributes = MemberAttributes.Private | MemberAttributes.Static,
                Name = "_default",
                Type = this.CurrentReference,
                InitExpression = this.CurrentReference.ToObjectCreateExpression(),
            };

            yield return currentInstanceField;

            var currentInstanceFieldRefExpr = this.CurrentReference.ToTypeReferenceExpression().ToFieldReference(currentInstanceField);

            yield return new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Type = this.CurrentReference,
                Name = "Default",
                HasGet = true,
                HasSet = true,
                GetStatements = { currentInstanceFieldRefExpr.ToMethodReturnStatement() },
                SetStatements = { new CodePropertySetValueReferenceExpression().ToAssignStatement(currentInstanceFieldRefExpr) }
            };
        }
    }
}
