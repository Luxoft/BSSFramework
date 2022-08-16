using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Automation.Utils.DatabaseUtils.Interfaces;

namespace Automation.Utils.DatabaseUtils
{
    public class DatabaseContextPool
    {
        public DatabaseContextPool(Func<IDatabaseContext> generationFunc)
        {
            generation = generationFunc;
        }

        public static DatabaseContextPool Initialize(Func<IDatabaseContext> generationFunc) => instance ??= new DatabaseContextPool(generationFunc);

        private readonly ConcurrentBag<IDatabaseContext> pool = new ConcurrentBag<IDatabaseContext>();

        private static DatabaseContextPool instance;

        private static Func<IDatabaseContext> generation;

        public static DatabaseContextPool Current => instance ?? throw new InvalidOperationException("Not initialized. Please use DatabaseContextPool.Initialize() first.");

        public virtual IDatabaseContext Get() => this.pool.TryTake(out IDatabaseContext item) ? item : generation.Invoke();

        public void Return(IDatabaseContext item) => this.pool.Add(item);

        public void Clear()
        {
            Parallel.ForEach(this.pool, x => x.Dispose());
            this.pool.Clear();
        }
    }
}