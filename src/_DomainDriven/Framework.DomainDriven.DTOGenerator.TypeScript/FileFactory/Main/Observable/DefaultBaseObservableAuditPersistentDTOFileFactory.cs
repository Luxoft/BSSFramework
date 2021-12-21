using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable
{
    /// <summary>
    /// Default base observable persistentAuditDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultBaseObservableAuditPersistentDTOFileFactory<TConfiguration> : ObservableFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultBaseObservableAuditPersistentDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.AuditPersistentDomainObjectBaseType )
        {
        }

        public override MainDTOFileType FileType => ObservableFileType.BaseObservableAuditPersistentDTO;

        public override CodeTypeReference BaseReference => this.Configuration.GetCodeTypeReference(this.DomainType, ObservableFileType.BaseObservablePersistentDTO);

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return this.GenerateObservableFromPlainJs();
            yield return this.GenerateStaticFromPlainJsMethod();
        }
    }
}
