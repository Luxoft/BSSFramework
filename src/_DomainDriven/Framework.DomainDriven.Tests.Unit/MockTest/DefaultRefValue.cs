namespace Framework.DomainDriven.UnitTest.MockTest
{
    public class DefaultRefValue : IRefValue
    {
        private readonly string _name;

        public DefaultRefValue(string name)
        {
            this._name = name;
        }

        public string Name { get { return this._name; } }
    }
}