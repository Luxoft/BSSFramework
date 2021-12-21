using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base
{
    /// <summary>
    /// ObservableDTO abstract file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public abstract class ObservableDTOFileFactory<TConfiguration> : ObservableFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        protected ObservableDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return this.GenerateStaticFromPlainJsMethod();

            yield return this.GenerateObservableFromPlainJs();
            yield return this.GenerateToJsonMethod();

            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.StrictDTO))
            {
                yield return this.GenerateObservableToStrictMethod();
            }
        }
    }
}
