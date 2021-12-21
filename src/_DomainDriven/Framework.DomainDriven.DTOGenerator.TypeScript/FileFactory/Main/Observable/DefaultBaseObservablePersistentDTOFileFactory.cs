using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable
{
    /// <summary>
    /// Default base observable persistentDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultBaseObservablePersistentDTOFileFactory<TConfiguration> : ObservableFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultBaseObservablePersistentDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.PersistentDomainObjectBaseType)
        {
        }

        public override MainDTOFileType FileType => ObservableFileType.BaseObservablePersistentDTO;

        public override CodeTypeReference BaseReference =>
         this.Configuration.GetCodeTypeReference(this.DomainType, ObservableFileType.BaseObservableAbstractDTO);

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return this.GenerateStaticFromPlainJsMethod();

            yield return this.GenerateObservableFromPlainJs();

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
                            new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), this.Configuration.Environment.IdentityProperty.Name).UnwrapObservableProperty()))
                }
            };
        }
    }
}
