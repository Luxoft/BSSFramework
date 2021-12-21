using System;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public interface IDBSessionFactory : IFactory<DBSessionMode, IDBSession>, IDisposable
    {
        AvailableValues AvailableValues { get; }

        event EventHandler<SessionFlushedEventArgs> SessionFlushed;
    }

    public static class DBSessionFactoryExtensions
    {
        public static void Evaluate([NotNull] this IFactory<DBSessionMode, IDBSession> sessionFactory,
            DBSessionMode sessionMode, [NotNull] Action<IDBSession> action)
        {
            if (sessionFactory == null) throw new ArgumentNullException(nameof(sessionFactory));
            if (action == null) throw new ArgumentNullException(nameof(action));

            sessionFactory.Evaluate(sessionMode, context =>
            {
                action(context);
                return default(object);
            });
        }

        public static TResult Evaluate<TResult>([NotNull] this IFactory<DBSessionMode, IDBSession> sessionFactory,
            DBSessionMode sessionMode, [NotNull] Func<IDBSession, TResult> getResult)
        {
            if (sessionFactory == null) throw new ArgumentNullException(nameof(sessionFactory));
            if (getResult == null) throw new ArgumentNullException(nameof(getResult));

            return sessionFactory.Create(sessionMode).Evaluate(getResult);
        }
    }
}
