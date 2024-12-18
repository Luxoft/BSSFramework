using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class SentMessageMap : ConfigurationBaseMap<SentMessage>
{
    public SentMessageMap()
    {
        this.Map(x => x.Comment);
        this.Map(x => x.ContextObjectId);
        this.Map(x => x.ContextObjectType);
        this.Map(x => x.Copy).Length(int.MaxValue);
        this.Map(x => x.From);
        this.Map(x => x.Message).Length(int.MaxValue);
        this.Map(x => x.ReplyTo).Length(int.MaxValue);
        this.Map(x => x.Subject).Length(1000);
        this.Map(x => x.TemplateName);
        this.Map(x => x.To).Length(int.MaxValue);
    }
}
