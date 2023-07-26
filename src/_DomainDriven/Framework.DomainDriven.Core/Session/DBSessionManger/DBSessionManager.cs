using Framework.Core;

namespace Framework.DomainDriven
{
    public class DBSessionManager : IDBSessionManager
    {
        private readonly ILazyObject<IDBSession> lazyDbSession;

        public DBSessionManager(ILazyObject<IDBSession> lazyDbSession) => this.lazyDbSession = lazyDbSession;

        /// <summary>
        /// Tries to close existing DB session (if exists) and to flush events to DAL listeners
        /// </summary>
        public async Task TryCloseDbSessionAsync(CancellationToken cancellationToken = default)
        {
            if (this.lazyDbSession.IsValueCreated)
            {
                await this.lazyDbSession.Value.CloseAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Tries to mark existing DB session (if exists) as faulted to prevent further DB writes
        /// </summary>
        public void TryFaultDbSession()
        {
            if (this.lazyDbSession.IsValueCreated)
            {
                this.lazyDbSession.Value.AsFault();
            }
        }
    }
}
