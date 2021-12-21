namespace Framework.CustomReports.Services.ExcelBuilder
{
    public struct HeaderDesignInfo
    {
        public HeaderDesignInfo(int maxDeep, int count) : this()
        {
            this.MaxDeep = maxDeep;
            this.Count = count;
        }

        public int MaxDeep { get; private set; }

        public int Count { get; private set; }
    }
}