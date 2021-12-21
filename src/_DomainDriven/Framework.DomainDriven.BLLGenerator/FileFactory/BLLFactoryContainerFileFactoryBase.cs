using System.CodeDom;

namespace Framework.DomainDriven.BLLGenerator
{
    public abstract class BLLFactoryContainerFileFactoryBase<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        protected BLLFactoryContainerFileFactoryBase(TConfiguration configuration)
            : base(configuration, null)
        {

        }


        public override FileType FileType => FileType.BLLFactoryContainer;


        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            yield return this.Configuration.Environment.BLLCore.GetCodeTypeReference(null, BLLCoreGenerator.FileType.BLLFactoryContainerInterface);
        }
    }
}