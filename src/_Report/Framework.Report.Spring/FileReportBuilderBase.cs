using System.Collections.Generic;
using System.IO;

namespace Framework.Report.Spring
{
    public abstract class FileReportBuilderBase
    {
        protected readonly ShiftEventSource evaluator = new ShiftEventSource(PrimitiveTemplateEvaluator.Default);


        protected FileReportBuilderBase()
        {
        }

        #region IFileTemplateEvaluator Members

        public object RootObject
        {
            get { return this.evaluator.Root; }
            set { this.evaluator.Root = value; }
        }

        public IReadOnlyDictionary<string, object> Variables
        {
            get { return this.evaluator.CollectionVars; }
            set
            {
                foreach (KeyValuePair<string, object> pair in value ?? new Dictionary<string, object>())
                {
                    this.evaluator.CollectionVars.Add(pair.Key, pair.Value);
                }
            }
        }


        public abstract void Execute(byte[] fileBytes, ref string targetFileName, bool evaluateExceptionRaise = false);

        protected void DeleteTargetFile(ref string targetFileName)
        {
            int count = 0;
            bool success = false;
            string targetFileNameTmp = targetFileName;
            while ((!success) && (count < 3))
            {
                try
                {
                    if (File.Exists(targetFileNameTmp))
                    {
                        File.Delete(targetFileNameTmp);
                    }
                    targetFileName = targetFileNameTmp;
                    success = true;
                }
                catch
                {
                    count++;
                    if (count >= 3)
                        throw;
                    targetFileNameTmp = Path.Combine(Path.GetDirectoryName(targetFileName), Path.GetFileNameWithoutExtension(targetFileName) + count.ToString() + Path.GetExtension(targetFileName));
                }
            }
        }

        #endregion
    }
}
