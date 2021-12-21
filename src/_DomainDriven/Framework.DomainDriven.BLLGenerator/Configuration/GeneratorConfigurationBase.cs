using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLLGenerator
{
    public abstract class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment, FileType>, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
    {
        protected GeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
        {
            this.Logics = LazyInterfaceImplementHelper.CreateProxy(this.GetLogics);
        }


        public virtual IBLLFactoryContainerGeneratorConfiguration Logics { get; }



        public CodeTypeReference BLLContextTypeReference => this.Environment.BLLCore.BLLContextInterfaceTypeReference;

        protected override string NamespacePostfix { get; } = "BLL";


        public virtual bool GenerateBllConstructor([NotNull] Type domainType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return true;
        }

        protected override IEnumerable<Type> GetDomainTypes()
        {
            return this.Environment.BLLCore.BLLDomainTypes;
        }

        protected virtual IBLLFactoryContainerGeneratorConfiguration GetLogics()
        {
            return new BLLFactoryContainerGeneratorConfiguration<GeneratorConfigurationBase<TEnvironment>>(this);
        }


        protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
        {
            yield return new CodeFileFactoryHeader<FileType>(FileType.BLL, "", domainType => domainType.Name + FileType.BLL);

            yield return new CodeFileFactoryHeader<FileType>(FileType.BLLFactory, "", domainType => domainType.Name + FileType.BLLFactory);

            yield return new CodeFileFactoryHeader<FileType>(FileType.DefaultBLLFactory, "", _ => this.Environment.TargetSystemName + FileType.DefaultBLLFactory);

            yield return new CodeFileFactoryHeader<FileType>(FileType.ImplementedBLLFactory, "", _ => this.Environment.TargetSystemName + FileType.ImplementedBLLFactory);

            yield return new CodeFileFactoryHeader<FileType>(FileType.BLLFactoryContainer, "", _ => this.Environment.TargetSystemName + FileType.BLLFactoryContainer);
        }
    }
}
