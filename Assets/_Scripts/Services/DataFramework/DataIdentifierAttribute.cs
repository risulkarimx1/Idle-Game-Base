namespace Services.DataFramework
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
    public class DataIdentifierAttribute : System.Attribute
    {
        public string Identifier { get; private set; }

        public DataIdentifierAttribute(string identifier)
        {
            Identifier = identifier;
        }
    }
}