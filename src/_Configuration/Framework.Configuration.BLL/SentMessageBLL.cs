using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class SentMessageBLL : DomainBLLBase<SentMessage>
    {
        public SentMessageBLL(IConfigurationBLLContext context)
            : base(context)
        {

        }
    }
}