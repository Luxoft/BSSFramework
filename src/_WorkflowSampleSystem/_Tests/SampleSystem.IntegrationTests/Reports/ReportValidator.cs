using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OfficeOpenXml;

namespace SampleSystem.IntegrationTests.Reports
{
    public class ReportValidator
    {
        private readonly ExcelWorksheet sheet;

        private readonly Dictionary<int, List<string>> valuesForValidate = new Dictionary<int, List<string>>();

        private readonly Dictionary<int, string> searchRowCriteria = new Dictionary<int, string>();

        private readonly Lazy<List<string>> headersLazy;

        private int headerRowNumber = 1;

        private int rowCount;

        public ReportValidator(ExcelWorksheet excelWorksheet)
        {
            this.sheet = excelWorksheet;
            this.headersLazy = new Lazy<List<string>>(() => this.sheet.GetHeaders(this.headerRowNumber));
        }

        private List<string> Header => this.headersLazy.Value;

        public ReportValidator AddSearchCriteria(string headerName, params string[] value)
        {
            if (this.Header.IndexOf(headerName) < 0)
            {
                throw new Exception($"No column with header name {headerName}");
            }

            return this.AddSearchCriteria(this.Header.IndexOf(headerName) + 1, value);
        }

        public ReportValidator AddSearchCriteria(int headerIndex, params string[] value)
        {
            if (!value.Any())
            {
                throw new Exception($"No specified search criteria for column '{this.Header[headerIndex - 1]}'");
            }

            this.valuesForValidate.Add(headerIndex, value.ToList());
            return this;
        }

        public ReportValidator SetHeaderRowNumber(int rowNumber)
        {
            this.headerRowNumber = rowNumber;
            return this;
        }

        public void ValidateRowCount(int expectedRowCount)
        {
            this.rowCount = this.sheet.RowCount();

            if (this.SetSearchRowCriteria(0))
            {
                this.rowCount = this.sheet.FindRows(this.searchRowCriteria).Count;
            }

            var info = this.searchRowCriteria.Join(Environment.NewLine, pair => $"{this.Header[pair.Key - 1]} = {pair.Value}");
            Assert.AreEqual(expectedRowCount, this.rowCount, string.Format("{0}Invalid Row Count for criteria {0}{1}", Environment.NewLine, info));
        }

        public void ValidateExists()
        {
            this.Validate(true);
        }

        public void ValidateNotExists()
        {
            this.Validate(false);
        }

        private void Validate(bool shouldExist)
        {
            var rowIndex = 0;

            while (this.SetSearchRowCriteria(rowIndex))
            {
                this.sheet.VerifyRowExists(this.searchRowCriteria, this.headerRowNumber, shouldExist);
                rowIndex++;
            }
        }

        private bool SetSearchRowCriteria(int rowIndex)
        {
            var rowExists = false;

            foreach (var column in this.valuesForValidate)
            {
                if (column.Value.Count <= rowIndex)
                {
                    continue;
                }

                rowExists = true;

                if (rowIndex == 0)
                {
                    this.searchRowCriteria.Add(column.Key, column.Value[rowIndex]);
                }
                else
                {
                    this.searchRowCriteria[column.Key] = column.Value[rowIndex];
                }
            }

            return rowExists;
        }
    }
}
