using System;

using JetBrains.Annotations;

using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public class GroupedValueDesign<T>
    {
        public GroupedValueDesign(string columnHeader, [NotNull] Action<T, ExcelRange> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            this.ColumnHeader = columnHeader;
            this.Action = action;
        }

        public string ColumnHeader { get; private set; }

        public Action<T, ExcelRange> Action { get; private set; }
    }
}
