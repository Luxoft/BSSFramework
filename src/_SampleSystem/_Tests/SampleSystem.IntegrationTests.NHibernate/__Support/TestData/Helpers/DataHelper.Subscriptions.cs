using Automation.ServiceEnvironment;

using Framework.Authorization.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

public partial class DataHelper
{
    public DomainType GetDomainType(Type domainObjectType)
    {
        return this.EvaluateRead(context =>
                                 {
                                     var bll = context.Configuration.Logics.DomainTypeFactory.Create();
                                     var result = bll.GetByType(domainObjectType);
                                     return result;
                                 });
    }

    public SecurityContextType GetSecurityContextType(Type domainObjectType)
    {
        return this.EvaluateRead(context =>
                                 {
                                     var bll = context.Authorization.Logics.SecurityContextType;
                                     var result = bll.GetObjectBy(x => x.Name == domainObjectType.Name);
                                     return result;
                                 });
    }

    public SubscriptionRecipientInfo GetRecipientsUntyped(
            Type domainObjectType,
            object prev,
            object next,
            string subscriptionCode)
    {
        return this.EvaluateRead(context =>
                                 {
                                     var bll = context.Configuration.Logics.Subscription;

                                     var result = bll.GetRecipientsUntyped(
                                                                           domainObjectType,
                                                                           prev,
                                                                           next,
                                                                           subscriptionCode);

                                     return result;
                                 });
    }

    public IList<ITryResult<Subscription>> ProcessChangedObjectUntyped(
            Type domainObjectType,
            object prev,
            object next)
    {
        return this.EvaluateWrite(context =>
                                  {
                                      var bll = context.Configuration.Logics.Subscription;

                                      var result = bll.ProcessChangedObjectUntyped(
                                                                                   prev,
                                                                                   next,
                                                                                   domainObjectType);

                                      return result;
                                  });
    }

    public IList<ITryResult<Subscription>> ProcessChangedObjectInfo(ObjectModificationInfo<Guid> changedObjectInfo)
    {
        return this.EvaluateWrite(context =>
                                  {
                                      var bll = context.Configuration.Logics.Subscription;
                                      var result = bll.Process(changedObjectInfo);
                                      return result;
                                  });
    }
}
