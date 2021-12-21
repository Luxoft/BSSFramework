namespace Framework.OData.Tests.Unit.DomainModel
{
    public struct NameEng
    {
        private string firstName;
        private string lastName;

        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = value; }
        }

        public string FullName
        {
            get { return this.FirstName + this.LastName; }
        }
    }
}
