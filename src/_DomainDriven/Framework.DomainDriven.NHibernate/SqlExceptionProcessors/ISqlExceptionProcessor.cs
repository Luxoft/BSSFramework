using System;

namespace Framework.DomainDriven.NHibernate.SqlExceptionProcessors
{

    internal interface ISqlExceptionProcessor
    {
        int ErrorNumber { get; }
        Exception Process(HandledGenericADOException genericAdoException, ExceptionProcessingContext context);
    }
}
