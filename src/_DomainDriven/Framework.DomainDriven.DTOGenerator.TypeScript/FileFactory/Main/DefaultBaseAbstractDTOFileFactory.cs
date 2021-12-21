using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main
{
    /// <summary>
    /// Default base abstractDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultBaseAbstractDTOFileFactory<TConfiguration> : BaseDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultBaseAbstractDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.DomainObjectBaseType)
        {
        }

        public override MainDTOFileType FileType => DTOGenerator.FileType.BaseAbstractDTO;

        public override CodeTypeReference BaseReference => null;

        protected override bool? InternalBaseTypeContainsPropertyChange => false;

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public
            };
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

            yield return CodeDomHelper.GenerateFromStaticInitializeMethodJs(this, true);
            yield return this.GenerateFromMethodsJs(true);

            yield return this.GenerateToNativeJsonMethod();
            yield return this.GenerateSelfToJson();

            if (this.FileType.AsObservableFileType() is var obsFileType && obsFileType != null && this.Configuration.GeneratePolicy.Used(this.DomainType, obsFileType))
            {
                yield return this.GenerateFromObservableMethod(true);
            }
        }
    }
}
