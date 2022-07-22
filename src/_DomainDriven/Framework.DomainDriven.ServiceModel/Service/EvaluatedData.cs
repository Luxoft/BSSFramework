using System;

using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.Service
{
    public class EvaluatedData<TBLLContext>
        where TBLLContext : class
    {
        public EvaluatedData([NotNull] IDBSession session, [NotNull] TBLLContext context)
        {
            this.Session = session ?? throw new ArgumentNullException(nameof(session));
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public IDBSession Session { get; }

        public TBLLContext Context { get; }
    }

    public class EvaluatedData<TBLLContext, TDTOMappingService> : EvaluatedData<TBLLContext>
        where TBLLContext : class
        where TDTOMappingService : class
    {
        public EvaluatedData([NotNull] IDBSession session, [NotNull] TBLLContext context, [NotNull] TDTOMappingService mappingService)
            : base(session, context)
        {
            this.MappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
        }


        public TDTOMappingService MappingService { get; }
    }
}
