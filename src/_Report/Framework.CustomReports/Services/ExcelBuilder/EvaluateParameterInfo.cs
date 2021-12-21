using System.Collections.Generic;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public struct EvaluateParameterInfo
    {
        public EvaluateParameterInfo(IList<EvaluateParameterInfoItem> items) : this()
        {
            this.Items = items;
        }

        public IList<EvaluateParameterInfoItem> Items { get; private set; }
    }
}
