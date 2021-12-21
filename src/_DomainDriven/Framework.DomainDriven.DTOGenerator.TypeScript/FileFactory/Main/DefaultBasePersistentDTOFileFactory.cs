using System.CodeDom;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main
{
    /// <summary>
    /// Default base persistentDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultBasePersistentDTOFileFactory<TConfiguration> : BaseDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultBasePersistentDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.PersistentDomainObjectBaseType)
        {
        }

        public override MainDTOFileType FileType => DTOGenerator.FileType.BasePersistentDTO;

        public override CodeTypeReference BaseReference => this.Configuration.GetBaseAbstractReference();

        protected override bool? InternalBaseTypeContainsPropertyChange => true;

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public,
            };
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }
        }

        protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
        {
            return Constants.InitializeByUndefined ? new CodePrimitiveExpression(null) : null;
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return CodeDomHelper.GenerateFromStaticInitializeMethodJs(this);

            yield return this.GenerateToStrictMethods(this.FileType);

            yield return this.GenerateFromMethodsJs();

            if (this.Configuration.GeneratePolicy.Used(this.DomainType, this.FileType.AsObservableFileType()))
            {
                yield return this.GenerateFromObservableMethod();
            }

            yield return new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public,
                Name = "IsNew",
                Type = new CodeTypeReference(typeof(bool)),
                GetStatements =
                {
                    new CodeMethodReturnStatement(
                        new CodeBinaryOperatorExpression(
                            new CodeDefaultValueExpression(CodeExpressionHelper.GetGuidCodeTypeReference()),
                            CodeBinaryOperatorType.IdentityEquality,
                            new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), this.Configuration.Environment.IdentityProperty.Name)))
                }
            };
        }
    }
}
