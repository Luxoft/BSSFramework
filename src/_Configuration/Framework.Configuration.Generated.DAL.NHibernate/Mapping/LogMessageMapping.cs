using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping
{
    public class LogMessageMapping : CfgBaseMap<LogMessage>
    {
        public LogMessageMapping()
        {
            this.Map(x => x.Action).Length(512);
            this.Map(x => x.InputMessage).Length(int.MaxValue);
            this.Map(x => x.OutputMessage).Length(int.MaxValue);
            this.Map(x => x.UserName).Length(512);
            this.Component(
                x => x.Period,
                part =>
                {
                    part.Map(x => x.EndDate).Column("periodendDate");
                    part.Map(x => x.StartDate).Column("periodstartDate");
                });
        }
    }
}
