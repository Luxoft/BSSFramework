namespace Framework.CustomReports.Domain
{
    public interface ICustomReportParameter
    {
        string Name{get;}
        string TypeName { get; }

        bool IsRequired { get; }
        
        int Order { get; }

        string DisplayValueProperty { get; }

        bool IsCollection { get; }
    }
}