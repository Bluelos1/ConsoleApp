namespace ConsoleApp.Model
{
    public class ImportedObject : ImportedObjectBaseClass
    {
        public string Name { get; set; }
        public string Schema { get; set; }

        public string ParentName { get; set; }
        public string ParentType { get; set; }

        public string DataType { get; set; }
        public string IsNullable { get; set; }

        public double NumberOfChildren { get; set; }
    }
}