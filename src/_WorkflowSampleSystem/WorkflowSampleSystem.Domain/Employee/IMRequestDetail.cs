namespace WorkflowSampleSystem.Domain
{
    /// <summary>
    /// Специальная деталь для проверки OneToOne-связи
    /// </summary>
    public class IMRequestDetail : AuditPersistentDomainObjectBase
    {
        private IMRequest request;

        protected IMRequestDetail()
        {
        }

        public IMRequestDetail(IMRequest request)
        {
            this.request = request;
        }

        public virtual IMRequest Request
        {
            get => this.request;
            protected internal set => this.request = value;
        }
    }
}
