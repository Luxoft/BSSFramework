namespace System.ServiceModel
{
    /// <summary>Fake for Generator</summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class OperationContractAttribute : Attribute
    {
        public string Name
        {
            get;
            set;
        }

        public string Action
        {
            get;
            set;
        }

        public string ReplyAction
        {
            get;
            set;
        }

        public bool IsOneWay
        {
            get;
            set;
        }
    }
}
