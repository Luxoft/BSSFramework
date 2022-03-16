using Framework.DomainDriven.NHibernate;

namespace AttachmentsSampleSystem.IntegrationTests.NH
{
    public class FullTextSearchHqlGeneratorsRegistry : EnhancedLinqToHqlGeneratorsRegistry
    {
        public FullTextSearchHqlGeneratorsRegistry()
        {
            // ReSharper disable RedundantBaseQualifier
#pragma warning disable SA1100 // Do not prefix calls with base unless local implementation exists

            base.RegisterGenerator(DialectExtensions.GetPropetyFullTextContainsMethodInfo(), new PropertyFullTextContainsGenerator());

#pragma warning restore SA1100 // Do not prefix calls with base unless local implementation exists

            // ReSharper restore RedundantBaseQualifier
        }
    }
}
