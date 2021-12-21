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
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (context == null) throw new ArgumentNullException(nameof(context));

            this.Session = session;
            this.Context = context;
        }


        public IDBSession Session { get; private set; }

        public TBLLContext Context { get; private set; }
    }

    public class EvaluatedData<TBLLContext, TDTOMappingService> : EvaluatedData<TBLLContext>
        where TBLLContext : class
        where TDTOMappingService : class
    {
        public EvaluatedData([NotNull] IDBSession session, [NotNull] TBLLContext context, [NotNull] TDTOMappingService mappingService)
            : base(session, context)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.MappingService = mappingService;
        }


        public TDTOMappingService MappingService { get; private set; }
    }
}
