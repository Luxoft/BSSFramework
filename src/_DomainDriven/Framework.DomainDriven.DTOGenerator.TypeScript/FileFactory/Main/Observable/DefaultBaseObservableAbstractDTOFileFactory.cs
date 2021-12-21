using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable
{
    /// <summary>
    /// Default base observable abstractDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultBaseObservableAbstractDTOFileFactory<TConfiguration> : ObservableFileFactory<TConfiguration> /*Base.BaseDTOFileFactory*/
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultBaseObservableAbstractDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.DomainObjectBaseType)
        {
        }

        public override MainDTOFileType FileType => ObservableFileType.BaseObservableAbstractDTO;

        public override CodeTypeReference BaseReference => null;

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public
            };
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return CodeDomHelper.GenerateFromStaticInitializeMethodJs(this, true);
            yield return this.GenerateFromMethodsJs(true);
            yield return this.GenerateFromObservableMethod(true);
        }

        protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
        {
            return null;
        }
    }
}
