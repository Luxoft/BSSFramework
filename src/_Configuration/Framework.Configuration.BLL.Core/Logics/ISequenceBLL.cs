using System;

namespace Framework.Configuration.BLL
{
    public partial interface ISequenceBLL
    {
        long GetNextNumber(string name);
    }

    public static class SequenceBLLExtensions
    {
        public static long GetNextNumber<T>(this ISequenceBLL sequenceBLL)
        {
            if (sequenceBLL == null) throw new ArgumentNullException(nameof(sequenceBLL));

            return sequenceBLL.GetNextNumber(typeof (T).Name);
        }
    }
}