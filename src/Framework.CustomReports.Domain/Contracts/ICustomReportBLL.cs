namespace Framework.CustomReports.Domain
{
    public interface ICustomReportBLL<TParameter> : ICustomReportEvaluator
    {
        IReportStream GetReportStream(TParameter parameter);
    }
}
