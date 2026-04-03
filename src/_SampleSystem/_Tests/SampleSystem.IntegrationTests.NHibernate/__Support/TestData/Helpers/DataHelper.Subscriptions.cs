using Framework.Authorization.Domain;
using Framework.AutomationCore.ServiceEnvironment.RootServiceProviderContainer;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Database.Domain;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

public partial class DataHelper
{
    public DomainType GetDomainType(Type domainObjectType) =>
        this.EvaluateRead(context =>
        {
            var bll = context.Configuration.Logics.DomainTypeFactory.Create();
            var result = bll.GetByType(domainObjectType);
            return result;
        });

    public SecurityContextType GetSecurityContextType(Type domainObjectType) =>
        this.EvaluateRead(context =>
        {
            var bll = context.Authorization.Logics.SecurityContextType;
            var result = bll.GetObjectBy(x => x.Name == domainObjectType.Name);
            return result;
        });

    public SubscriptionRecipientInfo GetRecipientsUntyped(
            Type domainObjectType,
            object prev,
            object next,
            string subscriptionCode) =>
        this.EvaluateRead(context =>
        {
            var bll = context.Configuration.Logics.Subscription;

            var result = bll.GetRecipientsUntyped(
                domainObjectType,
                prev,
                next,
                subscriptionCode);

            return result;
        });

    public List<ITryResult<Subscription>> ProcessChangedObjectUntyped(
            Type domainObjectType,
            object prev,
            object next) =>
        this.EvaluateWrite(context =>
        {
            var bll = context.Configuration.Logics.Subscription;

            var result = bll.ProcessChangedObjectUntyped(
                prev,
                next,
                domainObjectType);

            return result;
        });

    public List<ITryResult<Subscription>> ProcessChangedObjectInfo(ObjectModificationInfo<Guid> changedObjectInfo) =>
        this.EvaluateWrite(context =>
        {
            var bll = context.Configuration.Logics.Subscription;
            var result = bll.Process(changedObjectInfo);
            return result;
        });
}
