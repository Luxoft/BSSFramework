using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class SentMessageMap : ConfigurationBaseMap<SentMessage>
{
    public SentMessageMap()
    {
        this.Map(x => x.Comment).Not.Nullable();
        this.Map(x => x.ContextObjectId);
        this.Map(x => x.ContextObjectType).Not.Nullable();
        this.Map(x => x.Copy).Length(int.MaxValue).Not.Nullable();
        this.Map(x => x.From).Not.Nullable();
        this.Map(x => x.Message).Length(int.MaxValue).Not.Nullable();
        this.Map(x => x.ReplyTo).Length(int.MaxValue).Not.Nullable();
        this.Map(x => x.Subject).Length(1000).Not.Nullable();
        this.Map(x => x.TemplateName).Not.Nullable();
        this.Map(x => x.To).Length(int.MaxValue).Not.Nullable();
    }
}
