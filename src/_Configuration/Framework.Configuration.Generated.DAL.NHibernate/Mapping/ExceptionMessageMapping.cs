using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping
{
    public class ExceptionMessageMapping : CfgBaseMap<ExceptionMessage>
    {
        public ExceptionMessageMapping()
        {
            this.Map(x => x.IsClient);
            this.Map(x => x.IsRoot);
            this.Map(x => x.Message).Length(int.MaxValue);
            this.Map(x => x.MessageType);
            this.Map(x => x.StackTrace).Length(int.MaxValue);
            this.References(x => x.InnerException).Column($"{nameof(ExceptionMessage.InnerException)}Id");
        }
    }
}
