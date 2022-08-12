using System;
using System.Collections.Generic;
using System.Reflection;

using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3
{
    /// <summary>
    /// Предназначен для выполнения операций, связанных с подписками и версиями доменных объектов.
    /// </summary>
    /// <typeparam name="T">Тип доменных объектов, с которым будет работать экземпляр службы.</typeparam>
    /// <seealso cref="SubscriptionSystemService" />
    /// <seealso cref="IRevisionSubscriptionSystemService" />
    public class RevisionSubscriptionSystemService<TBLLContext, T> : SubscriptionSystemService<TBLLContext>, IRevisionSubscriptionSystemService
        where TBLLContext : class
        where T : class, IIdentityObject<Guid>
    {
        private readonly SubscriptionServicesFactory<TBLLContext, T> servicesFactory;

        /// <summary>
        /// Создаёт экземпляр класса <see cref="RevisionSubscriptionSystemService{T}"/>.
        /// </summary>
        /// <param name="servicesFactory">Фабрика служб, используемая <see cref="RevisionSubscriptionSystemService{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Аргумент servicesFactory равен null.</exception>
        public RevisionSubscriptionSystemService(
            [NotNull] SubscriptionServicesFactory<TBLLContext, T> servicesFactory)
            : base(servicesFactory)
        {
            if (servicesFactory == null)
            {
                throw new ArgumentNullException(nameof(servicesFactory));
            }

            this.servicesFactory = servicesFactory;
        }

        /// <summary>
        /// Возвращает данные об изменениях доменного объекта.
        /// </summary>
        /// <param name="changes">Описатель операций, проведенных над объектом в слое доступа к данным.</param>
        /// <returns>Экземпляр <see cref="IEnumerable{ObjectModificationInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">Аргумент changes равен null.</exception>
        public IEnumerable<ObjectModificationInfo<Guid>> GetObjectModifications(DALChanges changes)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            var service = this.CreateRevisionService<T>();
            var result = service.GetObjectModifications(changes);

            return result;
        }

        /// <summary>
        /// Выполняет рассылку уведомлений по всем подпискам, привязанным к типу изменяемого доменного объекта.
        /// </summary>
        /// <param name="changedObjectInfo">Данные об изменяемом доменном объекте.</param>
        /// <returns>Экземпляр <see cref="IList{ITryResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Аргумент changedObjectInfo равен null.</exception>
        public IList<ITryResult<Subscription>> Process([NotNull] ObjectModificationInfo<Guid> changedObjectInfo)
        {
            if (changedObjectInfo == null)
            {
                throw new ArgumentNullException(nameof(changedObjectInfo));
            }

            try
            {
                var facade = this.CreateConfigurationContextFacade();
                var domainObjectType = facade.GetDomainObjectType(changedObjectInfo.TypeInfo);

                var @delegate =
                    (Func<ObjectModificationInfo<Guid>, IList<ITryResult<Subscription>>>)this.ProcessTyped<T>;
                var method = @delegate.CreateGenericMethod(domainObjectType);
                var result = (IList<ITryResult<Subscription>>)method.Invoke(this, new object[] { changedObjectInfo });

                return result;
            }
            catch (TargetInvocationException ex)
            {
                return new[] { TryResult.CreateFault<Subscription>(ex.GetLastInnerException()) };
            }
            catch (Exception ex)
            {
                return new[] { TryResult.CreateFault<Subscription>(ex) };
            }
        }

        private IList<ITryResult<Subscription>> ProcessTyped<TDomainObject>(
            ObjectModificationInfo<Guid> info)
            where TDomainObject : class, T
        {
            var revisionService = this.CreateRevisionService<TDomainObject>();
            var notificationService = this.CreateNotificationService();

            var versions = revisionService.GetDomainObjectVersions(info.Identity, info.Revision);

            if (versions == null)
            {
                return new List<ITryResult<Subscription>>();
            }

            var result = notificationService.NotifyDomainObjectChanged(versions);
            return result;
        }

        private RevisionService<TDomainObject> CreateRevisionService<TDomainObject>()
            where TDomainObject : class, T
        {
            return this.servicesFactory.CreateRevisionService<TDomainObject>();
        }

        private ConfigurationContextFacade CreateConfigurationContextFacade()
        {
            return this.servicesFactory.CreateConfigurationContextFacade();
        }
    }
}
