namespace IIS.Product_19312
{
    public class Permissions
    {
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }

        public bool Full { get; set; }
        public bool Read { get; set; }
        public bool Insert { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Execute { get; set; }
    }
}