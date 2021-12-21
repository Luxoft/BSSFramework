namespace Framework.CustomReports
{
    
    public abstract class CustomReportBase<TSecurityOperation, TParameter>
    {
        private readonly TParameter parameter;
        private readonly TSecurityOperation securityOperation;

        protected CustomReportBase(TParameter parameter, TSecurityOperation securityOperation)
        {
            this.parameter = parameter;
            this.securityOperation = securityOperation;
        }

        public TParameter Parameter
        {
            get { return this.parameter; }
        }

        public TSecurityOperation SecurityOperation
        {
            get { return this.securityOperation; }
        }
    }
}
