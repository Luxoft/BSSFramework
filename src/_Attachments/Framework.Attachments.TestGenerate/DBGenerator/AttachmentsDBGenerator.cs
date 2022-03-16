using Framework.DomainDriven.DBGenerator;
using Framework.DomainDriven.NHibernate;

namespace Framework.Attachments.TestGenerate
{
    public class AttachmentsDBGenerator : DBGenerator
    {
        public AttachmentsDBGenerator(IMappingSettings settings)
            : base(settings)
        {

        }
    }
}
