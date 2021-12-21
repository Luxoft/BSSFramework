using System;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Generation
{
    public static class CheckOutServiceExtensions
    {
        public static ICheckOutService WithTrace(this ICheckOutService baseCheckOutService)
        {
            return new TraceCheckOutService(baseCheckOutService);
        }

        private class TraceCheckOutService : CheckOutService
        {
            private readonly ICheckOutService _baseCheckOutService;


            public TraceCheckOutService([NotNull] ICheckOutService baseCheckOutService)
            {
                this._baseCheckOutService = baseCheckOutService;

                if (baseCheckOutService == null) throw new ArgumentNullException(nameof(baseCheckOutService));
            }


            public override void CheckOutFile(string fileName)
            {
                if (fileName == null) throw new ArgumentNullException(nameof(fileName));

                try
                {
                    global::System.Diagnostics.Trace.WriteLine($"Try check out file '{fileName}'");

                    this._baseCheckOutService.CheckOutFile(fileName);

                    global::System.Diagnostics.Trace.WriteLine("Check out done");
                }
                catch (CheckOutServiceException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Unhandled error check out file '{fileName}'", ex);
                }
            }
        }
    }
}
