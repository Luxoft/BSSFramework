namespace Framework.OData.Tests.Unit.DomainModel
{
    public class Department : Base
    {
        private string name;
        private Location location;

        public Location Location { get { return this.location; } set { this.location = value; } }
        public string Name { get { return this.name; } }
    }
}
