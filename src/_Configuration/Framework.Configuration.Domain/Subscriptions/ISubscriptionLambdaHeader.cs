using Framework.Persistent;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Интерфейс для лямбд
    /// </summary>
    public interface ISubscriptionLambdaHeader :
        IDomainTypeElement<DomainType>,
        ITypeObject<SubscriptionLambdaType>,
        ITargetSystemElement<TargetSystem>,
        IAuthDomainTypeElement
    {
        bool WithContext { get; }
    }
}